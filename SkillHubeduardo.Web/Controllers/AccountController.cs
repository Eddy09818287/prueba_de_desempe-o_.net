using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Por favor ingresa tanto el correo como la contraseña.";
                return View();
            }

            var res = await _authService.LoginAsync(new LoginRequest(email.Trim(), password));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, res.UserId),
                new(ClaimTypes.Name, res.FullName),
                new(ClaimTypes.Email, res.Email),
                new(ClaimTypes.Role, res.Role),
                new("JwtToken", res.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["Success"] = $"¡Bienvenido de nuevo, {res.FullName}!";
            return RedirectToAction("Index", "Workshops");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string fullName, string email, string password)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View();
            }

            var res = await _authService.RegisterParticipantAsync(new RegisterRequest(fullName.Trim(), email.Trim(), password));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, res.UserId),
                new(ClaimTypes.Name, res.FullName),
                new(ClaimTypes.Email, res.Email),
                new(ClaimTypes.Role, res.Role),
                new("JwtToken", res.Token)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["Success"] = $"¡Cuenta creada exitosamente! Bienvenido, {res.FullName}.";
            return RedirectToAction("Index", "Workshops");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View();
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Success"] = "Has cerrado sesión correctamente.";
        return RedirectToAction("Index", "Workshops");
    }
}