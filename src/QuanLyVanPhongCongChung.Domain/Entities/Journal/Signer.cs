namespace QuanLyVanPhongCongChung.Domain.Entities.Journal;

using QuanLyVanPhongCongChung.Domain.Common;

public class Signer : BaseEntity
{
    public Guid JournalEntryId { get; private set; }
    public string FullName { get; private set; } = null!;

    public JournalEntry JournalEntry { get; private set; } = null!;

    private BiometricData? _biometricData = null;
    public BiometricData? BiometricData => _biometricData;

    private Signer() { }

    public static Signer Create(Guid journalEntryId, string fullName)
    {
        return new Signer
        {
            JournalEntryId = journalEntryId,
            FullName = fullName
        };
    }
}
