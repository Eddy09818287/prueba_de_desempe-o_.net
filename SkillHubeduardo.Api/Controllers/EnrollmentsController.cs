using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Enroll([FromBody] CreateEnrollmentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        try
        {
            var res = await _enrollmentService.EnrollParticipantAsync(userId, request.WorkshopId);
            return Ok(res);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("mine")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId);
        return Ok(enrollments);
    }

    [HttpPatch("{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelEnrollment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isEmployee = User.IsInRole("Employee");

        try
        {
            await _enrollmentService.CancelEnrollmentAsync(id, userId, isEmployee);
            return Ok(new { message = "Enrollment successfully cancelled." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("import")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Please upload a valid Excel (.xlsx) file." });

        using var stream = file.OpenReadStream();
        var result = await _enrollmentService.ImportEnrollmentsFromExcelAsync(stream);
        return Ok(result);
    }

    [HttpGet("{id}/certificate")]
    [Authorize]
    public async Task<IActionResult> DownloadCertificate(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isEmployee = User.IsInRole("Employee");

        try
        {
            var pdfBytes = await _enrollmentService.GenerateCertificateAsync(id, userId, isEmployee);
            return File(pdfBytes, "application/pdf", $"Constancia_SkillHubeduardo_{id}.pdf");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}