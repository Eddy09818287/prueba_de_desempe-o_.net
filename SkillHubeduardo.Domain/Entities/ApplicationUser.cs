using Microsoft.AspNetCore.Identity;

namespace SkillHubeduardo.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? Specialty { get; set; }
    public string Role { get; set; } = "Participant";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Workshop> InstructedWorkshops { get; set; } = new List<Workshop>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
