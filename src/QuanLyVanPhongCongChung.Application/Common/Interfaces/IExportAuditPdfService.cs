namespace QuanLyVanPhongCongChung.Application.Common.Interfaces;

using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public interface IExportAuditPdfService
{
    byte[] Generate(
        IReadOnlyCollection<ExportActivityLogItemDto> items,
        AuditReportSummaryDto summary,
        DateTimeOffset generatedAt);
}
