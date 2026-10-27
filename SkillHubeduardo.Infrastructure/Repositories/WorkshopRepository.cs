using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Enums;
using SkillHubeduardo.Domain.Interfaces;
using SkillHubeduardo.Infrastructure.Persistence;

namespace SkillHubeduardo.Infrastructure.Repositories;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly SkillHubeduardoDbContext _context;

    public WorkshopRepository(SkillHubeduardoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workshop>> GetFutureWorkshopsAsync(DateTime currentUtc)
    {
        return await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .Where(w => w.StartDate > currentUtc)
            .OrderBy(w => w.StartDate)
            .ToListAsync();
    }

    public async Task<Workshop?> GetByIdAsync(int id)
    {
        return await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Workshop?> GetByCodeAsync(string code)
    {
        return await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .FirstOrDefaultAsync(w => w.Code == code);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Workshops.AnyAsync(w => w.Code == code);
    }

    public async Task AddAsync(Workshop workshop)
    {
        await _context.Workshops.AddAsync(workshop);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Workshop>> GetTopWorkshopsAsync()
    {
        return await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .ToListAsync();
    }
}