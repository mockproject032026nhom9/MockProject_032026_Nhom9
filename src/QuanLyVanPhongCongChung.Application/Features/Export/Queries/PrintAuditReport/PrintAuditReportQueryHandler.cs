namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.PrintAuditReport;

using MediatR;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;
using QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

public class PrintAuditReportQueryHandler(
    IExportAuditLogReader exportAuditLogReader,
    IAuditReportFileBuilder auditReportFileBuilder,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<PrintAuditReportQuery, AuditReportDto>
{
    public async Task<AuditReportDto> Handle(PrintAuditReportQuery request, CancellationToken cancellationToken)
    {
        var format = request.Format.Trim().ToUpperInvariant();
        var generatedAt = dateTimeProvider.UtcNow;

        var filter = new ExportAuditLogFilter(
            request.From,
            request.To,
            request.DocumentId);

        var items = await exportAuditLogReader.ReadAsync(
            filter,
            skip: null,
            take: 5000,
            cancellationToken);

        var summary = new AuditReportSummaryDto(
            items.Count,
            items.Count(x => string.Equals(x.Status, "Verified", StringComparison.OrdinalIgnoreCase)),
            items.Count(x => string.Equals(x.Status, "Pending Auth", StringComparison.OrdinalIgnoreCase)),
            items.Count(x => string.Equals(x.Status, "Archived", StringComparison.OrdinalIgnoreCase)));

        var fileOutput = auditReportFileBuilder.Build(format, items, summary, generatedAt);

        return new AuditReportDto(
            generatedAt,
            format,
            summary,
            items,
            fileOutput.FileName,
            fileOutput.FileContentType,
            fileOutput.FileContentBase64,
            fileOutput.Note);
    }
}
