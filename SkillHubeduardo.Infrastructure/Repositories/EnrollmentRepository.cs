using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Enums;
using SkillHubeduardo.Domain.Interfaces;
using SkillHubeduardo.Infrastructure.Persistence;

namespace SkillHubeduardo.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly SkillHubeduardoDbContext _context;

    public EnrollmentRepository(SkillHubeduardoDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(e => e.Workshop)
                .ThenInclude(w => w!.Instructor)
            .Include(e => e.Participant)
            .Include(e => e.CancelledByUser)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Enrollment>> GetByWorkshopIdAsync(int workshopId)
    {
        return await _context.Enrollments
            .Include(e => e.Participant)
            .Include(e => e.Workshop)
            .Where(e => e.WorkshopId == workshopId)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetByParticipantIdAsync(string participantId)
    {
        return await _context.Enrollments
            .Include(e => e.Workshop)
                .ThenInclude(w => w!.Instructor)
            .Where(e => e.ParticipantId == participantId)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync();
    }

    public async Task<bool> HasActiveEnrollmentAsync(string participantId, int workshopId)
    {
        return await _context.Enrollments
            .AnyAsync(e => e.ParticipantId == participantId && e.WorkshopId == workshopId && e.Status == EnrollmentStatus.Active);
    }

    public async Task<bool> HasScheduleConflictAsync(string participantId, DateTime start, DateTime end, int? excludeWorkshopId = null)
    {
        // Conflict occurs if another active workshop overlaps: StartA < EndB and EndA > StartB
        return await _context.Enrollments
            .Include(e => e.Workshop)
            .Where(e => e.ParticipantId == participantId && e.Status == EnrollmentStatus.Active)
            .Where(e => excludeWorkshopId == null || e.WorkshopId != excludeWorkshopId.Value)
            .AnyAsync(e => e.Workshop!.StartDate < end && e.Workshop.EndDate > start);
    }

    public async Task AddAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }
}