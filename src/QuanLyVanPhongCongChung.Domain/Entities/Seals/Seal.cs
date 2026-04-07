namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Seal : BaseAuditableEntity, IAggregateRoot
{
    public Guid CommissionId { get; private set; }
    public SealType Type { get; private set; }
    public string Name { get; private set; } = null!;
    public SealStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public DateTimeOffset IssuedAt { get; private set; }
    public string? AllowedActTypes { get; private set; }
    public Guid? NotarialActId { get; private set; }
    public Guid? ReplaceSealId { get; private set; }

    public Notary.NotaryCommission Commission { get; private set; } = null!;
    public Seal? ReplaceSeal { get; private set; }

    private readonly List<SealUsageLog> _usageLogs = [];
    public IReadOnlyCollection<SealUsageLog> UsageLogs => _usageLogs.AsReadOnly();

    private Seal() { }

    public static Seal Create(Guid commissionId, SealType type, string name, DateTimeOffset issuedAt)
    {
        return new Seal
        {
            CommissionId = commissionId,
            Type = type,
            Name = name,
            IssuedAt = issuedAt,
            Status = SealStatus.Active
        };
    }

    public void Revoke() => Status = SealStatus.Revoked;
    public void Suspend() => Status = SealStatus.Suspended;
}
