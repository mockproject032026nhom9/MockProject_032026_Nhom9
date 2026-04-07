namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotaryAuditLog : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public string TableName { get; private set; } = null!;
    public Guid RecordId { get; private set; }
    public AuditAction Action { get; private set; }
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public string? ChangedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryAuditLog() { }

    public static NotaryAuditLog Create(Guid notaryId, string tableName, Guid recordId,
        AuditAction action, string? oldValue, string? newValue, string? changedBy)
    {
        return new NotaryAuditLog
        {
            NotaryId = notaryId,
            TableName = tableName,
            RecordId = recordId,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            ChangedBy = changedBy,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
