namespace SkillHubeduardo.Application.DTOs;

public record CreateEnrollmentRequest(int WorkshopId);

public record EnrollmentResponse(
    int Id,
    int WorkshopId,
    string WorkshopCode,
    string WorkshopTitle,
    DateTime WorkshopStartDate,
    DateTime WorkshopEndDate,
    string InstructorName,
    string ParticipantId,
    string ParticipantName,
    string ParticipantEmail,
    string Status,
    DateTime EnrolledAt,
    DateTime? CancelledAt,
    string? CancelledBy
);

public record ExcelImportRowError(int RowNumber, string Reason);

public record ExcelImportResult(
    int TotalProcessed,
    int SuccessfulCount,
    int RejectedCount,
    List<ExcelImportRowError> RejectedRows
);