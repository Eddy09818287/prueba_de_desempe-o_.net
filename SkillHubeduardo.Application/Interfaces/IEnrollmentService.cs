using SkillHubeduardo.Application.DTOs;

namespace SkillHubeduardo.Application.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentResponse> EnrollParticipantAsync(string participantId, int workshopId);
    Task<IEnumerable<EnrollmentResponse>> GetMyEnrollmentsAsync(string participantId);
    Task<IEnumerable<EnrollmentResponse>> GetWorkshopEnrollmentsAsync(int workshopId);
    Task CancelEnrollmentAsync(int enrollmentId, string currentUserId, bool isEmployee);
    Task<byte[]> GenerateCertificateAsync(int enrollmentId, string currentUserId, bool isEmployee);
    Task<ExcelImportResult> ImportEnrollmentsFromExcelAsync(Stream fileStream);
}