namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotaryStatusHistory : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public NotaryStatus Status { get; private set; }
    public string? Reason { get; private set; }
    public DateOnly EffectiveDate { get; private set; }
    public string? CreatedBy { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryStatusHistory() { }

    public static NotaryStatusHistory Create(Guid notaryId, NotaryStatus status,
        string? reason, DateOnly effectiveDate, string? createdBy)
    {
        return new NotaryStatusHistory
        {
            NotaryId = notaryId,
            Status = status,
            Reason = reason,
            EffectiveDate = effectiveDate,
            CreatedBy = createdBy
        };
    }
}
