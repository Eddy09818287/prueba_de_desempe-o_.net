using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Domain.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
}