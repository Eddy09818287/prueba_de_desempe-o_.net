using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;

    public ReportsController(IWorkshopService workshopService)
    {
        _workshopService = workshopService;
    }

    [HttpGet("top-workshops")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetTopWorkshops()
    {
        var report = await _workshopService.GetTopWorkshopsReportAsync();
        return Ok(report);
    }
}