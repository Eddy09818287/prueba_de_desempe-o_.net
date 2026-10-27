using SkillHubeduardo.Application.DTOs;

namespace SkillHubeduardo.Application.Interfaces;

public interface IWorkshopService
{
    Task<IEnumerable<WorkshopResponse>> GetAvailableWorkshopsAsync();
    Task<WorkshopResponse?> GetWorkshopByIdAsync(int id);
    Task<WorkshopResponse> CreateWorkshopAsync(CreateWorkshopRequest request);
    Task<IEnumerable<TopWorkshopReportDto>> GetTopWorkshopsReportAsync();
}