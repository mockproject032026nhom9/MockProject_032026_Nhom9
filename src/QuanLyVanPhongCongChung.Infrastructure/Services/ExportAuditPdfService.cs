namespace QuanLyVanPhongCongChung.Infrastructure.Services;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public class ExportAuditPdfService : IExportAuditPdfService
{
    static ExportAuditPdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Generate(
        IReadOnlyCollection<ExportActivityLogItemDto> items,
        AuditReportSummaryDto summary,
        DateTimeOffset generatedAt)
    {
        var generatedAtText = generatedAt.ToString("yyyy-MM-dd HH:mm:ss 'UTC'");

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Item().Text("Export Audit Report").Bold().FontSize(18);
                    column.Item().Text($"Generated At: {generatedAtText}").FontColor(Colors.Grey.Darken2);
                    column.Item().Text($"Total: {summary.TotalRecords} | Verified: {summary.VerifiedCount} | Pending Auth: {summary.PendingAuthCount} | Archived: {summary.ArchivedCount}")
                        .FontColor(Colors.Grey.Darken2);
                });

                page.Content().PaddingTop(10).Element(content =>
                {
                    content.Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1.8f); // User
                            columns.RelativeColumn(1.4f); // Role
                            columns.RelativeColumn(1.5f); // Document
                            columns.RelativeColumn(1.0f); // Format
                            columns.RelativeColumn(1.8f); // Timestamp
                            columns.RelativeColumn(1.1f); // Status
                            columns.RelativeColumn(1.4f); // Action
                        });

                        static IContainer HeaderCell(IContainer cell) => cell
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .PaddingVertical(6)
                            .PaddingHorizontal(4)
                            .Background(Colors.Grey.Lighten4);

                        static IContainer BodyCell(IContainer cell) => cell
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten3)
                            .PaddingVertical(5)
                            .PaddingHorizontal(4);

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("User").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Role").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Document ID").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Format").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Timestamp").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Status").SemiBold();
                            header.Cell().Element(HeaderCell).Text("Action").SemiBold();
                        });

                        foreach (var item in items)
                        {
                            table.Cell().Element(BodyCell).Text(item.UserName);
                            table.Cell().Element(BodyCell).Text(item.RoleName);
                            table.Cell().Element(BodyCell).Text(item.DocumentId);
                            table.Cell().Element(BodyCell).Text(item.Format);
                            table.Cell().Element(BodyCell).Text(item.Timestamp.ToString("yyyy-MM-dd HH:mm"));
                            table.Cell().Element(BodyCell).Text(item.Status);
                            table.Cell().Element(BodyCell).Text(item.Action);
                        }
                    });
                });

                page.Footer().AlignRight().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
        }).GeneratePdf();
    }
}
