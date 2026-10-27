using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Domain.Entities;
using SkillHubeduardo.Domain.Interfaces;
using SkillHubeduardo.Infrastructure.Persistence;

namespace SkillHubeduardo.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SkillHubeduardoDbContext _context;

    public UserRepository(SkillHubeduardoDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        return await _context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        return await _context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Email == email);
    }
}