using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Web.Controllers;

[Authorize]
public class EnrollmentsController : Controller
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Mine()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var enrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId);
        return View(enrollments);
    }

    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Enroll(int workshopId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        try
        {
            await _enrollmentService.EnrollParticipantAsync(userId, workshopId);
            TempData["Success"] = "¡Te has inscrito exitosamente al taller!";
            return RedirectToAction("Mine");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Details", "Workshops", new { id = workshopId });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id, string? returnUrl = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isEmployee = User.IsInRole("Employee");
        try
        {
            await _enrollmentService.CancelEnrollmentAsync(id, userId, isEmployee);
            TempData["Success"] = "Inscripción cancelada correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Mine");
    }

    public async Task<IActionResult> DownloadCertificate(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isEmployee = User.IsInRole("Employee");

        try
        {
            var bytes = await _enrollmentService.GenerateCertificateAsync(id, userId, isEmployee);
            return File(bytes, "application/pdf", $"Constancia_Taller_{id}.pdf");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Mine");
        }
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public IActionResult Import() => View();

    [Authorize(Roles = "Employee")]
    [HttpPost]
    public async Task<IActionResult> Import(IFormFile excelFile)
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            ViewBag.Error = "Por favor selecciona un archivo Excel (.xlsx) válido.";
            return View();
        }

        try
        {
            using var stream = excelFile.OpenReadStream();
            var result = await _enrollmentService.ImportEnrollmentsFromExcelAsync(stream);
            return View("ImportResult", result);
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Error al procesar el archivo: {ex.Message}";
            return View();
        }
    }
}