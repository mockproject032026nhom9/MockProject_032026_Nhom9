namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public interface IExportAuditLogReader
{
    Task<int> CountAsync(ExportAuditLogFilter filter, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<ExportActivityLogItemDto>> ReadAsync(
        ExportAuditLogFilter filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);
}
