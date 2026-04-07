namespace QuanLyVanPhongCongChung.Domain.Entities.Journal;

using QuanLyVanPhongCongChung.Domain.Common;

public class FeeBreakdown : BaseEntity
{
    public Guid JournalEntryId { get; private set; }
    public decimal BaseNotarialFee { get; private set; }
    public decimal ServiceFee { get; private set; }
    public decimal TravelFee { get; private set; }
    public decimal ConvenienceFee { get; private set; }
    public decimal RushFee { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal NotaryShare { get; private set; }
    public decimal CompanyShare { get; private set; }

    public JournalEntry JournalEntry { get; private set; } = null!;

    private FeeBreakdown() { }

    public static FeeBreakdown Create(Guid journalEntryId, decimal baseNotarialFee,
        decimal serviceFee, decimal travelFee, decimal convenienceFee, decimal rushFee,
        decimal notaryShare, decimal companyShare)
    {
        return new FeeBreakdown
        {
            JournalEntryId = journalEntryId,
            BaseNotarialFee = baseNotarialFee,
            ServiceFee = serviceFee,
            TravelFee = travelFee,
            ConvenienceFee = convenienceFee,
            RushFee = rushFee,
            TotalAmount = baseNotarialFee + serviceFee + travelFee + convenienceFee + rushFee,
            NotaryShare = notaryShare,
            CompanyShare = companyShare
        };
    }
}
