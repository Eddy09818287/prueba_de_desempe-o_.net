using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Interfaces;

namespace SkillHubeduardo.Application.Services;

public class WorkshopService : IWorkshopService
{
    private readonly IWorkshopRepository _workshopRepository;
    private readonly IUserRepository _userRepository;

    public WorkshopService(IWorkshopRepository workshopRepository, IUserRepository userRepository)
    {
        _workshopRepository = workshopRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<WorkshopResponse>> GetAvailableWorkshopsAsync()
    {
        var workshops = await _workshopRepository.GetFutureWorkshopsAsync(DateTime.UtcNow);
        return workshops.Select(MapToResponse);
    }

    public async Task<WorkshopResponse?> GetWorkshopByIdAsync(int id)
    {
        var w = await _workshopRepository.GetByIdAsync(id);
        return w == null ? null : MapToResponse(w);
    }

    public async Task<WorkshopResponse> CreateWorkshopAsync(CreateWorkshopRequest request)
    {
        var now = DateTime.UtcNow;

        if (request.StartDate <= now)
            throw new ArgumentException("Workshop start date must be in the future.");

        if (request.EndDate <= request.StartDate)
            throw new ArgumentException("Workshop end date must be strictly after the start date.");

        if (request.MaxCapacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.");

        if (await _workshopRepository.ExistsByCodeAsync(request.Code))
            throw new InvalidOperationException($"A workshop with code '{request.Code}' already exists.");

        var instructor = await _userRepository.GetByEmailAsync(request.InstructorEmail);
        if (instructor == null || instructor.Role != "Employee")
            throw new ArgumentException($"Instructor with email '{request.InstructorEmail}' does not exist or is not an employee.");

        var workshop = new Workshop
        {
            Code = request.Code.Trim().ToUpper(),
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            InstructorId = instructor.Id,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            MaxCapacity = request.MaxCapacity
        };

        await _workshopRepository.AddAsync(workshop);

        // Fetch back with navigation properties
        var created = await _workshopRepository.GetByIdAsync(workshop.Id);
        return MapToResponse(created!);
    }

    public async Task<IEnumerable<TopWorkshopReportDto>> GetTopWorkshopsReportAsync()
    {
        var workshops = await _workshopRepository.GetTopWorkshopsAsync();
        return workshops.Select(w => new TopWorkshopReportDto(
            w.Id,
            w.Code,
            w.Title,
            w.Instructor?.FullName ?? "N/A",
            w.StartDate,
            w.EndDate,
            w.MaxCapacity,
            w.Enrollments.Count(e => e.Status == Domain.Enums.EnrollmentStatus.Active)
        )).OrderByDescending(x => x.ActiveEnrollmentsCount);
    }

    private static WorkshopResponse MapToResponse(Workshop w) =>
        new(
            w.Id,
            w.Code,
            w.Title,
            w.Description,
            w.Instructor?.FullName ?? "N/A",
            w.Instructor?.Email ?? "N/A",
            w.Instructor?.Specialty,
            w.StartDate,
            w.EndDate,
            w.MaxCapacity,
            w.GetAvailableSpots()
        );
}