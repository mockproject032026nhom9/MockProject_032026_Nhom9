namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;

public class NotaryCapability : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public bool Mobile { get; private set; }
    public bool Ron { get; private set; }
    public bool LoanSigning { get; private set; }
    public bool ApostilleRelatedSupport { get; private set; }
    public decimal? MaxDistance { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private RonTechnology? _ronTechnology = null;
    public RonTechnology? RonTechnology => _ronTechnology;

    private NotaryCapability() { }

    public static NotaryCapability Create(Guid notaryId, bool mobile, bool ron,
        bool loanSigning, bool apostilleRelatedSupport, decimal? maxDistance = null)
    {
        return new NotaryCapability
        {
            NotaryId = notaryId,
            Mobile = mobile,
            Ron = ron,
            LoanSigning = loanSigning,
            ApostilleRelatedSupport = apostilleRelatedSupport,
            MaxDistance = maxDistance
        };
    }
}
