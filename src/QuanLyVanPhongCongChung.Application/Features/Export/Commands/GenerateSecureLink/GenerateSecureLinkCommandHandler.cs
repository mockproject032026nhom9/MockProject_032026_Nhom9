namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.GenerateSecureLink;

using System.Security.Cryptography;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;
using QuanLyVanPhongCongChung.Domain.Entities.Security;
using QuanLyVanPhongCongChung.Domain.Exceptions;

public class GenerateSecureLinkCommandHandler(
    IApplicationDbContext applicationDbContext,
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext,
    ICurrentUserService currentUserService,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<GenerateSecureLinkCommand, SecureExportLinkDto>
{
    public async Task<SecureExportLinkDto> Handle(GenerateSecureLinkCommand request, CancellationToken cancellationToken)
    {
        var requester = await ResolveRequesterAsync(cancellationToken);
        var requesterId = requester.UserId;
        var normalizedFormat = request.Format.Trim().ToUpperInvariant();
        var normalizedScope = request.Scope.Trim();

        if (!IsAllowedToGenerateScope(requester.RoleName, normalizedScope))
        {
            applicationDbContext.AuditLogs.Add(AuditLog.Create(
                requesterId,
                "GenerateSecureLinkDenied",
                nameof(ExportHistory),
                request.DocumentEntityId,
                ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                    request.DocumentId.Trim(),
                    normalizedFormat,
                    "Pending Auth",
                    "generate_secure_link_denied_scope"))));

            await applicationDbContext.SaveChangesAsync(cancellationToken);
            throw new DomainException("You are not allowed to generate secure link for this scope.");
        }

        var token = CreateUrlSafeToken();
        var expiresAt = dateTimeProvider.UtcNow.AddMinutes(request.ExpireInMinutes);

        var scopePayload = new ExportScopePayload(
            token,
            request.DocumentEntityId,
            request.DocumentId.Trim(),
            normalizedFormat,
            expiresAt,
            normalizedScope);

        var exportHistory = ExportHistory.Create(
            requesterId,
            ExportPayloadCodec.SerializeScope(scopePayload));

        applicationDbContext.ExportHistories.Add(exportHistory);

        var auditLog = AuditLog.Create(
            requesterId,
            "GenerateSecureLink",
            nameof(ExportHistory),
            exportHistory.Id,
            ExportPayloadCodec.SerializeAuditMetadata(new ExportAuditMetadataPayload(
                request.DocumentId.Trim(),
                normalizedFormat,
                "Verified",
                "generate_secure_link")));

        applicationDbContext.AuditLogs.Add(auditLog);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new SecureExportLinkDto(
            token,
            $"/api/exports/secure-links/{token}",
            expiresAt,
            request.DocumentId.Trim(),
            normalizedFormat);
    }

    private async Task<(Guid UserId, string RoleName)> ResolveRequesterAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(currentUserService.UserId, out var currentUserId))
            throw new DomainException("Authentication is required to generate secure link.");

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

    private static bool IsAllowedToGenerateScope(string roleName, string scope)
    {
        var isRegulatorScope = scope.Contains("regulator", StringComparison.OrdinalIgnoreCase);
        if (!isRegulatorScope)
            return true;

        return roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            || roleName.Equals("Compliance", StringComparison.OrdinalIgnoreCase)
            || roleName.Equals("LeadAuditor", StringComparison.OrdinalIgnoreCase);
    }

    private static string CreateUrlSafeToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
