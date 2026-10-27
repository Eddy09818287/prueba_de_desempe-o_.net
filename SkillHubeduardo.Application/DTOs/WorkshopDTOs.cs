namespace SkillHubeduardo.Application.DTOs;

public record WorkshopResponse(
    int Id,
    string Code,
    string Title,
    string Description,
    string InstructorName,
    string InstructorEmail,
    string? InstructorSpecialty,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    int AvailableSpots
);

public record CreateWorkshopRequest(
    string Code,
    string Title,
    string Description,
    string InstructorEmail,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity
);

public record TopWorkshopReportDto(
    int WorkshopId,
    string Code,
    string Title,
    string InstructorName,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    int ActiveEnrollmentsCount
);