namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;

public class NotaryBond : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public string ProviderName { get; private set; } = null!;
    public decimal BondAmount { get; private set; }
    public DateOnly EffectiveDate { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public string? FileUrl { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryBond() { }

    public static NotaryBond Create(Guid notaryId, string providerName, decimal bondAmount,
        DateOnly effectiveDate, DateOnly expirationDate)
    {
        return new NotaryBond
        {
            NotaryId = notaryId,
            ProviderName = providerName,
            BondAmount = bondAmount,
            EffectiveDate = effectiveDate,
            ExpirationDate = expirationDate
        };
    }
}
