using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Domain.Interfaces;

public interface IWorkshopRepository
{
    Task<IEnumerable<Workshop>> GetFutureWorkshopsAsync(DateTime currentUtc);
    Task<Workshop?> GetByIdAsync(int id);
    Task<Workshop?> GetByCodeAsync(string code);
    Task<bool> ExistsByCodeAsync(string code);
    Task AddAsync(Workshop workshop);
    Task<IEnumerable<Workshop>> GetTopWorkshopsAsync();
}