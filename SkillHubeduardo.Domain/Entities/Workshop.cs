using SkillHubeduardo.Domain.Enums;

namespace SkillHubeduardo.Domain.Entities;

public class Workshop
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // e.g. FOT-001
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string InstructorId { get; set; } = string.Empty;
    public ApplicationUser? Instructor { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public int GetAvailableSpots()
    {
        var activeCount = Enrollments.Count(e => e.Status == EnrollmentStatus.Active);
        return Math.Max(0, MaxCapacity - activeCount);
    }

    public bool HasStarted(DateTime currentUtc)
    {
        return currentUtc >= StartDate;
    }
}