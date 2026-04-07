namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

using System.Text;
using System.Text.Json;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public class AuditReportFileBuilder(
    IExportAuditPdfService exportAuditPdfService) : IAuditReportFileBuilder
{
    public AuditReportFileResult Build(
        string format,
        IReadOnlyCollection<ExportActivityLogItemDto> items,
        AuditReportSummaryDto summary,
        DateTimeOffset generatedAt)
    {
        var normalizedFormat = format.Trim().ToUpperInvariant();
        var stamp = generatedAt.ToString("yyyyMMddHHmmss");

        if (normalizedFormat == "CSV")
            return BuildCsv(items, stamp);

        if (normalizedFormat == "JSON")
            return BuildJson(items, stamp);

        var pdfBytes = exportAuditPdfService.Generate(items, summary, generatedAt);
        return new AuditReportFileResult(
            $"audit-report-{stamp}.pdf",
            "application/pdf",
            Convert.ToBase64String(pdfBytes),
            null);
    }

    private static AuditReportFileResult BuildCsv(IReadOnlyCollection<ExportActivityLogItemDto> items, string stamp)
    {
        var sb = new StringBuilder();
        sb.AppendLine("AuditLogId,UserName,RoleName,DocumentId,Format,Timestamp,Status,Action");

        foreach (var item in items)
        {
            sb.AppendLine(
                string.Join(",",
                    EscapeCsv(item.AuditLogId.ToString()),
                    EscapeCsv(item.UserName),
                    EscapeCsv(item.RoleName),
                    EscapeCsv(item.DocumentId),
                    EscapeCsv(item.Format),
                    EscapeCsv(item.Timestamp.ToString("O")),
                    EscapeCsv(item.Status),
                    EscapeCsv(item.Action)));
        }

        return new AuditReportFileResult(
            $"audit-report-{stamp}.csv",
            "text/csv",
            ExportPayloadCodec.ToBase64(sb.ToString()),
            null);
    }

    private static AuditReportFileResult BuildJson(IReadOnlyCollection<ExportActivityLogItemDto> items, string stamp)
    {
        var content = JsonSerializer.Serialize(items, new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        });

        return new AuditReportFileResult(
            $"audit-report-{stamp}.json",
            "application/json",
            ExportPayloadCodec.ToBase64(content),
            null);
    }

    private static string EscapeCsv(string value)
        => $"\"{value.Replace("\"", "\"\"")}\"";
}
