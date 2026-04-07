namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportActivityLogs;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record GetExportActivityLogsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    DateTimeOffset? From = null,
    DateTimeOffset? To = null,
    string? DocumentId = null,
    string? Format = null,
    string? Status = null) : IRequest<PagedResultDto<ExportActivityLogItemDto>>;
