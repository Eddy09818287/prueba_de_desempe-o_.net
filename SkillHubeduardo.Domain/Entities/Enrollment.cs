using SkillHubeduardo.Domain.Enums;

namespace SkillHubeduardo.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }
    public int WorkshopId { get; set; }
    public Workshop? Workshop { get; set; }
    public string ParticipantId { get; set; } = string.Empty;
    public ApplicationUser? Participant { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    
    // Cancellation audit
    public DateTime? CancelledAt { get; set; }
    public string? CancelledByUserId { get; set; }
    public ApplicationUser? CancelledByUser { get; set; }

    public void Cancel(string cancelledByUserId, bool isEmployee, DateTime currentUtc)
    {
        if (Status == EnrollmentStatus.Cancelled)
            throw new InvalidOperationException("Enrollment is already cancelled.");

        if (Workshop != null && Workshop.HasStarted(currentUtc))
            throw new InvalidOperationException("Cannot cancel enrollment for a workshop that has already started.");

        if (!isEmployee)
        {
            // Participant can only cancel up to 24 hours before start date
            if (Workshop != null && (Workshop.StartDate - currentUtc).TotalHours < 24)
            {
                throw new InvalidOperationException("Participants can only cancel up to 24 hours prior to workshop start time.");
            }
        }

        Status = EnrollmentStatus.Cancelled;
        CancelledAt = currentUtc;
        CancelledByUserId = cancelledByUserId;
    }
}