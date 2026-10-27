using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillHubeduardo.Application.DTOs;
using SkillHubeduardo.Application.Interfaces;
using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }

    public async Task<AuthResponse> RegisterParticipantAsync(RegisterRequest request)
    {
        var email = request.Email.Trim();
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
            throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");

        // Ensure "Participant" role exists
        if (!await _roleManager.RoleExistsAsync("Participant"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Participant"));
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = request.FullName.Trim(),
            Role = "Participant",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Error al registrar usuario: {errors}");
        }

        await _userManager.AddToRoleAsync(user, "Participant");

        var token = GenerateJwtToken(user, "Participant");
        return new AuthResponse(token, user.Id, user.FullName, user.Email!, "Participant");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var email = request.Email.Trim();
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Also check by UserName in case
            user = await _userManager.FindByNameAsync(email);
        }

        if (user == null)
            throw new UnauthorizedAccessException("Credenciales incorrectas: El correo electrónico no está registrado.");

        // Direct password check via PasswordHasher (avoids SignInManager lockout / email confirmation quirks)
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Credenciales incorrectas: La contraseña ingresada no es válida.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? user.Role ?? "Participant";

        var token = GenerateJwtToken(user, role);
        return new AuthResponse(token, user.Id, user.FullName, user.Email!, role);
    }

    private string GenerateJwtToken(ApplicationUser user, string role)
    {
        var jwtSettings = _config.GetSection("Jwt");
        var secretKey = jwtSettings["Key"] ?? "SkillHubSuperSecureSecretKey_2026_Minimum32CharsLength!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email ?? ""),
            new(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "SkillHubeduardoApi",
            audience: jwtSettings["Audience"] ?? "SkillHubeduardoClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}