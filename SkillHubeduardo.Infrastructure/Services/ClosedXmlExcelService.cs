using ClosedXML.Excel;
using SkillHubeduardo.Application.Interfaces;

namespace SkillHubeduardo.Infrastructure.Services;

public class ClosedXmlExcelService : IExcelService
{
    public List<EnrollmentImportItem> ParseEnrollmentFile(Stream stream)
    {
        var result = new List<EnrollmentImportItem>();
        using var workbook = new XLWorkbook(stream);
        
        // Find sheet Enrollments or use first worksheet
        IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name.Equals("Enrollments", StringComparison.OrdinalIgnoreCase))
                                 ?? workbook.Worksheets.First();

        var firstRow = worksheet.FirstRowUsed();
        if (firstRow == null) return result;

        int emailCol = -1;
        int codeCol = -1;

        // Detect columns from header
        foreach (var cell in firstRow.Cells())
        {
            var header = cell.GetString().Trim();
            if (header.Equals("ParticipantEmail", StringComparison.OrdinalIgnoreCase) || header.Equals("Email", StringComparison.OrdinalIgnoreCase))
                emailCol = cell.Address.ColumnNumber;
            else if (header.Equals("WorkshopCode", StringComparison.OrdinalIgnoreCase) || header.Equals("Code", StringComparison.OrdinalIgnoreCase))
                codeCol = cell.Address.ColumnNumber;
        }

        // Fallback default column positions 1 and 2 if headers not strictly named
        if (emailCol == -1) emailCol = 1;
        if (codeCol == -1) codeCol = 2;

        var rows = worksheet.RowsUsed().Skip(1); // skip header
        foreach (var row in rows)
        {
            var email = row.Cell(emailCol).GetString().Trim();
            var code = row.Cell(codeCol).GetString().Trim();

            // Ignore totally empty lines
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(code))
                continue;

            result.Add(new EnrollmentImportItem(row.RowNumber(), email, code));
        }

        return result;
    }
}