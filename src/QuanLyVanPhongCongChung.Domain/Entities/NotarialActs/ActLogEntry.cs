namespace QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

using QuanLyVanPhongCongChung.Domain.Common;

public class ActLogEntry : BaseEntity
{
    public Guid ActId { get; private set; }
    public Guid NotaryId { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    public NotarialAct Act { get; private set; } = null!;
    public Notary.Notary Notary { get; private set; } = null!;

    private ActLogEntry() { }

    public static ActLogEntry Create(Guid actId, Guid notaryId)
    {
        return new ActLogEntry
        {
            ActId = actId,
            NotaryId = notaryId,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}
