namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportAccessControl;

using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public class GetExportAccessControlQueryHandler(
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext) : IRequestHandler<GetExportAccessControlQuery, ExportAccessControlDto>
{
    public async Task<ExportAccessControlDto> Handle(GetExportAccessControlQuery request, CancellationToken cancellationToken)
    {
        var latestLog = await readOnlyApplicationDbContext.AuditLogs
            .Where(x => x.EntityType == "ExportAccessControl" && x.EntityId == request.DocumentEntityId)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);

        var payload = ExportPayloadCodec.DeserializeAccessControl(latestLog?.Metadata);

        return new ExportAccessControlDto(
            request.DocumentEntityId,
            payload?.DocumentId ?? request.DocumentId ?? request.DocumentEntityId.ToString(),
            payload?.ClientAccessEnabled ?? true,
            payload?.RegulatorAccessEnabled ?? false,
            payload?.ComplianceNotificationTriggered ?? false);
    }
}
