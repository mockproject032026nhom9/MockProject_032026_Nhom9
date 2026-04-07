namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;

public class NotaryInsurance : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public string PolicyNumber { get; private set; } = null!;
    public string ProviderName { get; private set; } = null!;
    public decimal CoverageAmount { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public string? FileUrl { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryInsurance() { }

    public static NotaryInsurance Create(Guid notaryId, string policyNumber, string providerName,
        decimal coverageAmount, DateOnly expirationDate)
    {
        return new NotaryInsurance
        {
            NotaryId = notaryId,
            PolicyNumber = policyNumber,
            ProviderName = providerName,
            CoverageAmount = coverageAmount,
            ExpirationDate = expirationDate
        };
    }
}
