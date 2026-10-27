using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Domain.Interfaces;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByIdAsync(int id);
    Task<IEnumerable<Enrollment>> GetByWorkshopIdAsync(int workshopId);
    Task<IEnumerable<Enrollment>> GetByParticipantIdAsync(string participantId);
    Task<bool> HasActiveEnrollmentAsync(string participantId, int workshopId);
    Task<bool> HasScheduleConflictAsync(string participantId, DateTime start, DateTime end, int? excludeWorkshopId = null);
    Task AddAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
}