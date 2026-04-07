namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportActivityLogs;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;
using QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

public class GetExportActivityLogsQueryHandler(
    IExportAuditLogReader exportAuditLogReader) : IRequestHandler<GetExportActivityLogsQuery, PagedResultDto<ExportActivityLogItemDto>>
{
    public async Task<PagedResultDto<ExportActivityLogItemDto>> Handle(GetExportActivityLogsQuery request, CancellationToken cancellationToken)
    {
        var filter = new ExportAuditLogFilter(
            From: request.From,
            To: request.To,
            DocumentId: request.DocumentId,
            Format: request.Format,
            Status: request.Status);

        var totalCount = await exportAuditLogReader.CountAsync(filter, cancellationToken);
        var items = await exportAuditLogReader.ReadAsync(
            filter,
            skip: (request.PageNumber - 1) * request.PageSize,
            take: request.PageSize,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return new PagedResultDto<ExportActivityLogItemDto>(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount,
            totalPages);
    }
}
