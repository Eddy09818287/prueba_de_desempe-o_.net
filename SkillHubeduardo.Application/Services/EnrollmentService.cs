using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Enums;
using SkillHubeduardo.Domain.Interfaces;

namespace SkillHubeduardo.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepo;
    private readonly IWorkshopRepository _workshopRepo;
    private readonly IUserRepository _userRepo;
    private readonly IPdfGenerator _pdfGenerator;
    private readonly IExcelService _excelService;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepo,
        IWorkshopRepository workshopRepo,
        IUserRepository userRepo,
        IPdfGenerator pdfGenerator,
        IExcelService excelService)
    {
        _enrollmentRepo = enrollmentRepo;
        _workshopRepo = workshopRepo;
        _userRepo = userRepo;
        _pdfGenerator = pdfGenerator;
        _excelService = excelService;
    }

    public async Task<EnrollmentResponse> EnrollParticipantAsync(string participantId, int workshopId)
    {
        var user = await _userRepo.GetByIdAsync(participantId);
        if (user == null)
            throw new KeyNotFoundException("Participant not found.");

        var workshop = await _workshopRepo.GetByIdAsync(workshopId);
        if (workshop == null)
            throw new KeyNotFoundException("Workshop not found.");

        var now = DateTime.UtcNow;

        // Rule 4: Future workshops only
        if (workshop.HasStarted(now))
            throw new InvalidOperationException("Cannot enroll in a workshop that has already started.");

        // Rule 1: Capacity
        if (workshop.GetAvailableSpots() <= 0)
            throw new InvalidOperationException("Workshop is full. No available spots.");

        // Rule 2: No duplicates active
        if (await _enrollmentRepo.HasActiveEnrollmentAsync(participantId, workshopId))
            throw new InvalidOperationException("Participant already has an active enrollment in this workshop.");

        // Rule 3: No schedule overlap with another active workshop
        if (await _enrollmentRepo.HasScheduleConflictAsync(participantId, workshop.StartDate, workshop.EndDate, workshop.Id))
            throw new InvalidOperationException("Schedule conflict: Participant has another active workshop at the same time.");

        var enrollment = new Enrollment
        {
            WorkshopId = workshopId,
            ParticipantId = participantId,
            Status = EnrollmentStatus.Active,
            EnrolledAt = now
        };

        await _enrollmentRepo.AddAsync(enrollment);

        var loaded = await _enrollmentRepo.GetByIdAsync(enrollment.Id);
        return MapToResponse(loaded!);
    }

    public async Task<IEnumerable<EnrollmentResponse>> GetMyEnrollmentsAsync(string participantId)
    {
        var enrollments = await _enrollmentRepo.GetByParticipantIdAsync(participantId);
        return enrollments.Select(MapToResponse);
    }

    public async Task<IEnumerable<EnrollmentResponse>> GetWorkshopEnrollmentsAsync(int workshopId)
    {
        var enrollments = await _enrollmentRepo.GetByWorkshopIdAsync(workshopId);
        return enrollments.Select(MapToResponse);
    }

    public async Task CancelEnrollmentAsync(int enrollmentId, string currentUserId, bool isEmployee)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
        if (enrollment == null)
            throw new KeyNotFoundException("Enrollment not found.");

        if (!isEmployee && enrollment.ParticipantId != currentUserId)
            throw new UnauthorizedAccessException("You are not allowed to cancel this enrollment.");

        enrollment.Cancel(currentUserId, isEmployee, DateTime.UtcNow);
        await _enrollmentRepo.UpdateAsync(enrollment);
    }

    public async Task<byte[]> GenerateCertificateAsync(int enrollmentId, string currentUserId, bool isEmployee)
    {
        var enrollment = await _enrollmentRepo.GetByIdAsync(enrollmentId);
        if (enrollment == null)
            throw new KeyNotFoundException("Enrollment not found.");

        if (!isEmployee && enrollment.ParticipantId != currentUserId)
            throw new UnauthorizedAccessException("You can only download certificates for your own enrollments.");

        if (enrollment.Status != EnrollmentStatus.Active)
            throw new InvalidOperationException("Certificates are only issued for active enrollments.");

        return _pdfGenerator.GenerateEnrollmentCertificate(enrollment, DateTime.UtcNow);
    }

    public async Task<ExcelImportResult> ImportEnrollmentsFromExcelAsync(Stream fileStream)
    {
        var items = _excelService.ParseEnrollmentFile(fileStream);
        var errors = new List<ExcelImportRowError>();
        int successCount = 0;

        foreach (var item in items)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.ParticipantEmail))
                {
                    errors.Add(new ExcelImportRowError(item.RowIndex, "Participant email is empty."));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.WorkshopCode))
                {
                    errors.Add(new ExcelImportRowError(item.RowIndex, "Workshop code is empty."));
                    continue;
                }

                var participant = await _userRepo.GetByEmailAsync(item.ParticipantEmail.Trim());
                if (participant == null)
                {
                    errors.Add(new ExcelImportRowError(item.RowIndex, $"Participant '{item.ParticipantEmail}' does not exist."));
                    continue;
                }

                var workshop = await _workshopRepo.GetByCodeAsync(item.WorkshopCode.Trim());
                if (workshop == null)
                {
                    errors.Add(new ExcelImportRowError(item.RowIndex, $"Workshop with code '{item.WorkshopCode}' does not exist."));
                    continue;
                }

                await EnrollParticipantAsync(participant.Id, workshop.Id);
                successCount++;
            }
            catch (Exception ex)
            {
                errors.Add(new ExcelImportRowError(item.RowIndex, ex.Message));
            }
        }

        return new ExcelImportResult(items.Count, successCount, errors.Count, errors);
    }

    private static EnrollmentResponse MapToResponse(Enrollment e) =>
        new(
            e.Id,
            e.WorkshopId,
            e.Workshop?.Code ?? "N/A",
            e.Workshop?.Title ?? "N/A",
            e.Workshop?.StartDate ?? DateTime.MinValue,
            e.Workshop?.EndDate ?? DateTime.MinValue,
            e.Workshop?.Instructor?.FullName ?? "N/A",
            e.ParticipantId,
            e.Participant?.FullName ?? "N/A",
            e.Participant?.Email ?? "N/A",
            e.Status.ToString(),
            e.EnrolledAt,
            e.CancelledAt,
            e.CancelledByUser?.FullName
        );
}