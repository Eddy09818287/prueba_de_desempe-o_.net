namespace SkillHubeduardo.Application.Interfaces;

public record EnrollmentImportItem(int RowIndex, string ParticipantEmail, string WorkshopCode);

public interface IExcelService
{
    List<EnrollmentImportItem> ParseEnrollmentFile(Stream stream);
}