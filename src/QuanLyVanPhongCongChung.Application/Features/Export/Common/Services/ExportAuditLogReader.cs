namespace QuanLyVanPhongCongChung.Application.Features.Export.Common.Services;

using Microsoft.EntityFrameworkCore;
using QuanLyVanPhongCongChung.Application.Common.Interfaces;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public class ExportAuditLogReader(
    IReadOnlyApplicationDbContext readOnlyApplicationDbContext) : IExportAuditLogReader
{
    public Task<int> CountAsync(ExportAuditLogFilter filter, CancellationToken cancellationToken)
        => BuildQuery(filter).CountAsync(cancellationToken);

    public async Task<IReadOnlyCollection<ExportActivityLogItemDto>> ReadAsync(
        ExportAuditLogFilter filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken)
    {
        var query = BuildQuery(filter)
            .OrderByDescending(x => x.Timestamp)
            .Select(x => new AuditLogProjection(x.Id, x.UserId, x.Action, x.Timestamp, x.Metadata));

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        var logs = await query.ToListAsync(cancellationToken);

        var userIds = logs
            .Where(x => x.UserId.HasValue)
            .Select(x => x.UserId!.Value)
            .Distinct()
            .ToList();

        var users = await readOnlyApplicationDbContext.Users
            .Where(x => userIds.Contains(x.Id))
            .Select(x => new UserProjection(x.Id, x.FullName, x.RoleId))
            .ToListAsync(cancellationToken);

        var roleIds = users.Select(x => x.RoleId).Distinct().ToList();

        var roles = await readOnlyApplicationDbContext.Roles
            .Where(x => roleIds.Contains(x.Id))
            .Select(x => new RoleProjection(x.Id, x.RoleName))
            .ToListAsync(cancellationToken);

        var userById = users.ToDictionary(x => x.Id, x => x);
        var roleById = roles.ToDictionary(x => x.Id, x => x.RoleName);

        return logs.Select(log => MapLog(log, userById, roleById)).ToList();
    }

    private IQueryable<QuanLyVanPhongCongChung.Domain.Entities.Security.AuditLog> BuildQuery(ExportAuditLogFilter filter)
    {
        var query = readOnlyApplicationDbContext.AuditLogs.AsQueryable();

        if (filter.From.HasValue)
            query = query.Where(x => x.Timestamp >= filter.From.Value);

        if (filter.To.HasValue)
            query = query.Where(x => x.Timestamp <= filter.To.Value);

        if (!string.IsNullOrWhiteSpace(filter.DocumentId))
        {
            var value = filter.DocumentId.Trim();
            query = query.Where(x => x.Metadata != null && x.Metadata.Contains(value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Format))
        {
            var value = filter.Format.Trim();
            query = query.Where(x => x.Metadata != null && x.Metadata.Contains(value));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var value = filter.Status.Trim();
            query = query.Where(x => x.Metadata != null && x.Metadata.Contains(value));
        }

        return query;
    }

    private static ExportActivityLogItemDto MapLog(
        AuditLogProjection log,
        IReadOnlyDictionary<Guid, UserProjection> userById,
        IReadOnlyDictionary<Guid, string> roleById)
    {
        var metadata = ExportPayloadCodec.DeserializeAuditMetadata(log.Metadata);

        string userName;
        string roleName;

        if (log.UserId.HasValue && userById.TryGetValue(log.UserId.Value, out var user))
        {
            userName = user.FullName;
            roleName = roleById.TryGetValue(user.RoleId, out var role) ? role : "Unknown";
        }
        else
        {
            userName = "System Bot";
            roleName = "System";
        }

        return new ExportActivityLogItemDto(
            log.Id,
            userName,
            roleName,
            metadata?.DocumentId ?? log.Id.ToString(),
            metadata?.Format ?? "N/A",
            log.Timestamp,
            metadata?.Status ?? "Verified",
            log.Action);
    }

    private sealed record AuditLogProjection(
        Guid Id,
        Guid? UserId,
        string Action,
        DateTimeOffset Timestamp,
        string? Metadata);

    private sealed record UserProjection(Guid Id, string FullName, Guid RoleId);

    private sealed record RoleProjection(Guid Id, string RoleName);
}
