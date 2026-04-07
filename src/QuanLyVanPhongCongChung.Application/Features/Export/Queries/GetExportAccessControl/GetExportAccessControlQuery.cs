namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportAccessControl;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record GetExportAccessControlQuery(
    Guid DocumentEntityId,
    string? DocumentId = null) : IRequest<ExportAccessControlDto>;
