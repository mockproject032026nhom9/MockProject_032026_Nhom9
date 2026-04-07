namespace QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class ActSignature : BaseEntity
{
    public Guid ActId { get; private set; }
    public Guid UserId { get; private set; }
    public int OrderIndex { get; private set; }
    public string? SignatureData { get; private set; }
    public SignatureStatus Status { get; private set; }

    public NotarialAct Act { get; private set; } = null!;
    public Identity.User User { get; private set; } = null!;

    private ActSignature() { }

    public static ActSignature Create(Guid actId, Guid userId, int orderIndex)
    {
        return new ActSignature
        {
            ActId = actId,
            UserId = userId,
            OrderIndex = orderIndex,
            Status = SignatureStatus.Pending
        };
    }

    public void Sign(string signatureData)
    {
        SignatureData = signatureData;
        Status = SignatureStatus.Signed;
    }
}
