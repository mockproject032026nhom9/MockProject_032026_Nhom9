namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetSecureExportByToken;

using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Exceptions;

public class GetSecureExportByTokenQueryHandler(
    IApplicationDbContext applicationDbContext,
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext,
    ICurrentUserService currentUserService,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<GetSecureExportByTokenQuery, SecureExportAccessDto>
{
    public async Task<SecureExportAccessDto> Handle(GetSecureExportByTokenQuery request, CancellationToken cancellationToken)
    {
        var token = request.Token.Trim();

        var candidates = await readOnlyApplicationDbContext.ExportHistories
            .Where(x => x.ExportScope != null && x.ExportScope.Contains(token))
            .OrderByDescending(x => x.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        var matched = candidates
            .Select(x => new { ExportHistory = x, Payload = ExportPayloadCodec.DeserializeScope(x.ExportScope) })
            .FirstOrDefault(x => x.Payload is not null && x.Payload.Token == token);

        if (matched is null || matched.Payload is null)
            throw new NotFoundException("Secure export link was not found.");

        if (matched.Payload.ExpiresAt <= dateTimeProvider.UtcNow)
            throw new DomainException("Secure export link has expired.");

        var accessControlLog = await readOnlyApplicationDbContext.AuditLogs
            .Where(x => x.EntityType == "ExportAccessControl" && x.EntityId == matched.Payload.DocumentEntityId)
            .OrderByDescending(x => x.Timestamp)
            .FirstOrDefaultAsync(cancellationToken);

        var accessControlPayload = ExportPayloadCodec.DeserializeAccessControl(accessControlLog?.Metadata);
        var clientAccessEnabled = accessControlPayload?.ClientAccessEnabled ?? true;
        var regulatorAccessEnabled = accessControlPayload?.RegulatorAccessEnabled ?? false;

        var userId = Guid.TryParse(currentUserService.UserId, out var parsedUserId)
            ? parsedUserId
            : (Guid?)null;

        var roleName = userId.HasValue
            ? await ResolveRoleNameAsync(userId.Value, cancellationToken)
            : null;

        if (IsRegulatorScope(matched.Payload.Scope) && !IsAllowedRegulatorConsumer(roleName))
        {
            applicationDbContext.AuditLogs.Add(AuditLog.Create(
                userId,
                "AccessSecureLinkDenied",
                nameof(ExportHistory),
                matched.ExportHistory.Id,
                ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                    matched.Payload.DocumentId,
                    matched.Payload.Format,
                    "Pending Auth",
                    "access_secure_link_denied_role"))));

            await applicationDbContext.SaveChangesAsync(cancellationToken);
            throw new DomainException("Authentication and role are required for regulator scope access.");
        }

        var allowAccess = IsScopeAllowed(matched.Payload.Scope, clientAccessEnabled, regulatorAccessEnabled);

        if (!allowAccess)
        {
            applicationDbContext.AuditLogs.Add(AuditLog.Create(
                userId,
                "AccessSecureLink",
                nameof(ExportHistory),
                matched.ExportHistory.Id,
                ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                    matched.Payload.DocumentId,
                    matched.Payload.Format,
                    "Pending Auth",
                    "access_secure_link_denied"))));

            await applicationDbContext.SaveChangesAsync(cancellationToken);
            throw new DomainException("Secure export access is disabled for this scope.");
        }

        applicationDbContext.AuditLogs.Add(AuditLog.Create(
            userId,
            "AccessSecureLink",
            nameof(ExportHistory),
            matched.ExportHistory.Id,
            ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                matched.Payload.DocumentId,
                matched.Payload.Format,
                "Verified",
                "access_secure_link"))));

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new SecureExportAccessDto(
            matched.ExportHistory.Id,
            matched.Payload.DocumentEntityId,
            matched.Payload.DocumentId,
            matched.Payload.Format,
            matched.Payload.ExpiresAt,
            false,
            matched.Payload.Scope);
    }

    private static bool IsScopeAllowed(string? scope, bool clientAccessEnabled, bool regulatorAccessEnabled)
    {
        if (string.IsNullOrWhiteSpace(scope))
            return clientAccessEnabled;

        var normalized = scope.Trim();
        var isRegulatorScope = normalized.Contains("regulator", StringComparison.OrdinalIgnoreCase);
        var isClientScope = normalized.Contains("client", StringComparison.OrdinalIgnoreCase);

        if (isRegulatorScope && !regulatorAccessEnabled)
            return false;

        if (isClientScope && !clientAccessEnabled)
            return false;

        if (!isRegulatorScope && !isClientScope)
            return clientAccessEnabled;

        return true;
    }

    private static bool IsRegulatorScope(string? scope)
        => !string.IsNullOrWhiteSpace(scope)
           && scope.Contains("regulator", StringComparison.OrdinalIgnoreCase);

    private static bool IsAllowedRegulatorConsumer(string? roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return false;

        return roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || roleName.Equals("Compliance", StringComparison.OrdinalIgnoreCase)
            || roleName.Equals("LeadAuditor", StringComparison.OrdinalIgnoreCase)
            || roleName.Equals("Regulator", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<string?> ResolveRoleNameAsync(Guid userId, CancellationToken cancellationToken)
    {
        var roleId = await readOnlyApplicationDbContext.Users
            .Where(x => x.Id == userId)
            .Select(x => (Guid?)x.RoleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!roleId.HasValue)
            return null;

        return await readOnlyApplicationDbContext.Roles
            .Where(x => x.Id == roleId.Value)
            .Select(x => x.RoleName)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
