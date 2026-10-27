using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;
    private readonly IEnrollmentService _enrollmentService;

    public WorkshopsController(IWorkshopService workshopService, IEnrollmentService enrollmentService)
    {
        _workshopService = workshopService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetWorkshops()
    {
        var list = await _workshopService.GetAvailableWorkshopsAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetWorkshopById(int id)
    {
        var workshop = await _workshopService.GetWorkshopByIdAsync(id);
        if (workshop == null) return NotFound(new { message = "Workshop not found." });
        return Ok(workshop);
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> CreateWorkshop([FromBody] CreateWorkshopRequest request)
    {
        try
        {
            var created = await _workshopService.CreateWorkshopAsync(request);
            return CreatedAtAction(nameof(GetWorkshopById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/enrollments")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> GetWorkshopEnrollments(int id)
    {
        var enrollments = await _enrollmentService.GetWorkshopEnrollmentsAsync(id);
        return Ok(enrollments);
    }
}