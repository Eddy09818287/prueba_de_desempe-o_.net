using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Infrastructure.Persistence;

public class SkillHubeduardoDbContext : IdentityDbContext<ApplicationUser>
{
    public SkillHubeduardoDbContext(DbContextOptions<SkillHubeduardoDbContext> options) : base(options)
    {
    }

    public DbSet<Workshop> Workshops => Set<Workshop>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Workshop entity configuration
        builder.Entity<Workshop>(b =>
        {
            b.HasKey(w => w.Id);
            b.Property(w => w.Code).IsRequired().HasMaxLength(50);
            b.HasIndex(w => w.Code).IsUnique();
            b.Property(w => w.Title).IsRequired().HasMaxLength(200);
            b.Property(w => w.Description).IsRequired().HasMaxLength(2000);
            
            b.HasOne(w => w.Instructor)
                .WithMany(u => u.InstructedWorkshops)
                .HasForeignKey(w => w.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Enrollment entity configuration
        builder.Entity<Enrollment>(b =>
        {
            b.HasKey(e => e.Id);
            
            b.HasOne(e => e.Workshop)
                .WithMany(w => w.Enrollments)
                .HasForeignKey(e => e.WorkshopId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(e => e.Participant)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(e => e.CancelledByUser)
                .WithMany()
                .HasForeignKey(e => e.CancelledByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
