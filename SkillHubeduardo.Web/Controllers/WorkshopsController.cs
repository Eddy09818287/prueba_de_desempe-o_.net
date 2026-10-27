using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Web.Controllers;

public class WorkshopsController : Controller
{
    private readonly IWorkshopService _workshopService;
    private readonly IEnrollmentService _enrollmentService;

    public WorkshopsController(IWorkshopService workshopService, IEnrollmentService enrollmentService)
    {
        _workshopService = workshopService;
        _enrollmentService = enrollmentService;
    }

    public async Task<IActionResult> Index()
    {
        var workshops = await _workshopService.GetAvailableWorkshopsAsync();
        return View(workshops);
    }

    public async Task<IActionResult> Details(int id)
    {
        var workshop = await _workshopService.GetWorkshopByIdAsync(id);
        if (workshop == null) return NotFound();
        return View(workshop);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public IActionResult Create() => View();

    [Authorize(Roles = "Employee")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateWorkshopRequest request)
    {
        try
        {
            var created = await _workshopService.CreateWorkshopAsync(request);
            TempData["Success"] = $"¡Taller '{created.Title}' ({created.Code}) publicado exitosamente!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(request);
        }
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> Attendees(int id)
    {
        var workshop = await _workshopService.GetWorkshopByIdAsync(id);
        if (workshop == null) return NotFound();

        var enrollments = await _enrollmentService.GetWorkshopEnrollmentsAsync(id);
        ViewBag.Workshop = workshop;
        return View(enrollments);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> TopReport()
    {
        var report = await _workshopService.GetTopWorkshopsReportAsync();
        return View(report);
    }
}