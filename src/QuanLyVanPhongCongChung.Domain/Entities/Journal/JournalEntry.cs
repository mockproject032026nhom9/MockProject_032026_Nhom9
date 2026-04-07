namespace QuanLyVanPhongCongChung.Domain.Entities.Journal;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class JournalEntry : BaseAuditableEntity, IAggregateRoot
{
    public Guid NotaryId { get; private set; }
    public decimal NotarialFee { get; private set; }
    public JournalEntryStatus Status { get; private set; }

    public Notary.Notary Notary { get; private set; } = null!;

    private readonly List<Signer> _signers = [];
    public IReadOnlyCollection<Signer> Signers => _signers.AsReadOnly();

    private FeeBreakdown? _feeBreakdown = null;
    public FeeBreakdown? FeeBreakdown => _feeBreakdown;

    private JournalEntry() { }

    public static JournalEntry Create(Guid notaryId, decimal notarialFee)
    {
        return new JournalEntry
        {
            NotaryId = notaryId,
            NotarialFee = notarialFee,
            Status = JournalEntryStatus.Draft
        };
    }

    public void Lock() => Status = JournalEntryStatus.Locked;
    public void Complete() => Status = JournalEntryStatus.Completed;
}
