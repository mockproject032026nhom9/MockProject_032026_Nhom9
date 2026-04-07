namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.UpdateExportAccessControl;

using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Exceptions;

public class UpdateExportAccessControlCommandHandler(
    IApplicationDbContext applicationDbContext,
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateExportAccessControlCommand, ExportAccessControlDto>
{
    public async Task<ExportAccessControlDto> Handle(UpdateExportAccessControlCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await ResolveCurrentUserAsync(cancellationToken);

        var latestLog = await readOnlyApplicationDbContext.AuditLogs
            .Where(x => x.EntityType == "ExportAccessControl" && x.EntityId == request.DocumentEntityId)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);

        var previous = ExportPayloadCodec.DeserializeAccessControl(latestLog?.Metadata);
        var previousRegulatorEnabled = previous?.RegulatorAccessEnabled ?? false;

        if (!previousRegulatorEnabled && request.RegulatorAccessEnabled && !IsAllowedToEnableRegulatorAccess(currentUser.RoleName))
        {
            applicationDbContext.AuditLogs.Add(AuditLog.Create(
                currentUser.UserId,
                "UpdateExportAccessControlDenied",
                "ExportAccessControl",
                request.DocumentEntityId,
                ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                    request.DocumentId.Trim(),
                    "N/A",
                    "Pending Auth",
                    "update_access_control_denied_regulator_enable"))));

            await applicationDbContext.SaveChangesAsync(cancellationToken);
            throw new DomainException("You are not allowed to enable regulator access.");
        }

        var complianceNotificationTriggered = !previousRegulatorEnabled && request.RegulatorAccessEnabled;

        var metadata = ExportPayloadCodec.SerializeAccessControl(new ExportAccessControlPayload(
            request.DocumentId.Trim(),
            request.ClientAccessEnabled,
            request.RegulatorAccessEnabled,
            complianceNotificationTriggered));

        applicationDbContext.AuditLogs.Add(AuditLog.Create(
            currentUser.UserId,
            "UpdateExportAccessControl",
            "ExportAccessControl",
            request.DocumentEntityId,
            metadata));

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new ExportAccessControlDto(
            request.DocumentEntityId,
            request.DocumentId.Trim(),
            request.ClientAccessEnabled,
            request.RegulatorAccessEnabled,
            complianceNotificationTriggered);
    }

    private async Task<(Guid UserId, string RoleName)> ResolveCurrentUserAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(currentUserService.UserId, out var currentUserId))
            throw new DomainException("Authentication is required to update export access control.");

        var user = await readOnlyApplicationDbContext.Users
            .Where(x => x.Id == currentUserId)
            .Select(x => new { x.Id, x.RoleId })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            throw new DomainException("Authenticated user was not found.");

        var roleName = await readOnlyApplicationDbContext.Roles
            .Where(x => x.Id == user.RoleId)
            .Select(x => x.RoleName)
            .FirstOrDefaultAsync(cancellationToken);

        return (user.Id, roleName ?? "Unknown");
    }

    private static bool IsAllowedToEnableRegulatorAccess(string roleName)
        => roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase)
           || roleName.Equals("Compliance", StringComparison.OrdinalIgnoreCase)
           || roleName.Equals("LeadAuditor", StringComparison.OrdinalIgnoreCase);
}
