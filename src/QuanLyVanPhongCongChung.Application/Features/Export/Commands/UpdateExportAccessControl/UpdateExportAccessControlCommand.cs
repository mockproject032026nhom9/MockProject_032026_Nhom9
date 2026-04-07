namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.UpdateExportAccessControl;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record UpdateExportAccessControlCommand(
    Guid DocumentEntityId,
    string DocumentId,
    bool ClientAccessEnabled,
    bool RegulatorAccessEnabled) : IRequest<ExportAccessControlDto>;
