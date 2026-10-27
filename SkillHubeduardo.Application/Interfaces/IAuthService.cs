using SkillHubeduardo.Application.DTOs;

namespace SkillHubeduardo.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterParticipantAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}