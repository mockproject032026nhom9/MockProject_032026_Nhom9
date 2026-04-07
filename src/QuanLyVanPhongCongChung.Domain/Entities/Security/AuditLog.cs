namespace QuanLyVanPhongCongChung.Domain.Entities.Security;

using QuanLyVanPhongCongChung.Domain.Common;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = null!;
    public string EntityType { get; private set; } = null!;
    public Guid EntityId { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public string? Metadata { get; private set; }

    public Identity.User? User { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(Guid? userId, string action, string entityType,
        Guid entityId, string? metadata = null)
    {
        return new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Timestamp = DateTimeOffset.UtcNow,
            Metadata = metadata
        };
    }
}
