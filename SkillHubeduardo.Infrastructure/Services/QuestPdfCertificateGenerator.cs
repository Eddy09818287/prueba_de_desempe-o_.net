using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkillHubeduardo.Application.Interfaces;
using SkillHubeduardo.Domain.Entities;

namespace SkillHubeduardo.Infrastructure.Services;

public class QuestPdfCertificateGenerator : IPdfGenerator
{
    public byte[] GenerateEnrollmentCertificate(Enrollment enrollment, DateTime generationDate)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var workshop = enrollment.Workshop!;
        var participant = enrollment.Participant!;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header()
                    .BorderBottom(2)
                    .BorderColor("#1e3a8a")
                    .PaddingBottom(10)
                    .Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("SkillHubeduardo").Bold().FontSize(28).FontColor("#1e3a8a");
                            col.Item().Text("Training & Workshops Center").FontSize(10).FontColor("#64748b");
                        });
                        row.ConstantItem(200).AlignRight().Column(col =>
                        {
                            col.Item().Text("CONSTANCIA OFICIAL").Bold().FontSize(12).FontColor("#1e3a8a");
                            col.Item().Text($"Código: {workshop.Code}").FontSize(9).FontColor("#64748b");
                        });
                    });

                page.Content().PaddingVertical(25).Column(col =>
                {
                    col.Item().AlignCenter().Text("CONSTANCIA DE INSCRIPCIÓN").Bold().FontSize(22).FontColor("#0f172a");
                    col.Item().PaddingTop(15).Text(text =>
                    {
                        text.Span("SkillHubeduardo hace constar que el participante ");
                        text.Span(participant.FullName).Bold();
                        text.Span($" (Correo: {participant.Email}) se encuentra formalmente inscrito y con cupo confirmado en el taller:");
                    });

                    col.Item().PaddingVertical(15).Background("#f8fafc").Padding(15).Column(info =>
                    {
                        info.Item().Text($"Taller: {workshop.Title}").Bold().FontSize(15).FontColor("#1e3a8a");
                        info.Item().Text($"Código Único: {workshop.Code}").FontSize(11);
                        info.Item().Text($"Instructor Responsable: {workshop.Instructor?.FullName ?? "Instructor Asignado"}").FontSize(11);
                        info.Item().Text($"Fecha y Horario: {workshop.StartDate:dd/MM/yyyy HH:mm} hasta {workshop.EndDate:dd/MM/yyyy HH:mm}").FontSize(11);
                        info.Item().Text($"Fecha de Inscripción: {enrollment.EnrolledAt:dd/MM/yyyy HH:mm:ss} UTC").FontSize(10).Italic();
                    });

                    col.Item().PaddingTop(10).Text("Esta constancia certifica la reserva activa del cupo para las fechas indicadas. En caso de no poder asistir, recuerde gestionar su cancelación con anticipación.")
                        .FontSize(9).FontColor("#475569");
                });

                page.Footer()
                    .BorderTop(1)
                    .BorderColor("#cbd5e1")
                    .PaddingTop(10)
                    .Row(row =>
                    {
                        row.RelativeItem().Text($"Fecha de Generación: {generationDate:dd/MM/yyyy HH:mm:ss} UTC").FontSize(9).FontColor("#94a3b8");
                        row.RelativeItem().AlignRight().Text("SkillHubeduardo Platform © Todos los derechos reservados").FontSize(9).FontColor("#94a3b8");
                    });
            });
        });

        return document.GeneratePdf();
    }
}