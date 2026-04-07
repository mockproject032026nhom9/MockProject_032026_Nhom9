namespace QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

using QuanLyVanPhongCongChung.Domain.Common;

public class ComplianceReview : BaseEntity
{
    public Guid ActId { get; private set; }
    public string? Result { get; private set; }

    public NotarialAct Act { get; private set; } = null!;

    private ComplianceReview() { }

    public static ComplianceReview Create(Guid actId, string? result = null)
    {
        return new ComplianceReview
        {
            ActId = actId,
            Result = result
        };
    }
}
