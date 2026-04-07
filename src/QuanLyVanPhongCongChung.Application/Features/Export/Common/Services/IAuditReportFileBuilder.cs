namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public interface IAuditReportFileBuilder
{
    AuditReportFileResult Build(
        string format,
        IReadOnlyCollection<ExportActivityLogItemDto> items,
        AuditReportSummaryDto summary,
        DateTimeOffset generatedAt);
}
