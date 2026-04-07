namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;

public class SealUsageLog : BaseEntity
{
    public Guid SealId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTimeOffset UsedAt { get; private set; }
    public int? PageNumber { get; private set; }
    public bool IsAnomaly { get; private set; }

    public Seal Seal { get; private set; } = null!;
    public Identity.User User { get; private set; } = null!;

    private SealUsageLog() { }

    public static SealUsageLog Create(Guid sealId, Guid userId, int? pageNumber = null, bool isAnomaly = false)
    {
        return new SealUsageLog
        {
            SealId = sealId,
            UserId = userId,
            UsedAt = DateTimeOffset.UtcNow,
            PageNumber = pageNumber,
            IsAnomaly = isAnomaly
        };
    }
}
