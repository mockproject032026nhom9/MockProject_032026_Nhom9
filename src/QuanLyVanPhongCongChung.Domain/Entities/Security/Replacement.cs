namespace QuanLyVanPhongCongChung.Domain.Entities.Security;

using QuanLyVanPhongCongChung.Domain.Common;

public class Replacement : BaseEntity
{
    public Guid? OldSealId { get; private set; }
    public Guid? OldCertificateId { get; private set; }
    public Guid? NewSealId { get; private set; }
    public Guid? NewCertificateId { get; private set; }
    public DateTimeOffset ReplacedAt { get; private set; }
    public Guid PerformedBy { get; private set; }
    public Guid? RevocationId { get; private set; }

    public Seals.Seal? OldSeal { get; private set; }
    public Seals.Seal? NewSeal { get; private set; }
    public Seals.Certificate? OldCertificate { get; private set; }
    public Seals.Certificate? NewCertificate { get; private set; }
    public Identity.User PerformedByUser { get; private set; } = null!;
    public Revocation? Revocation { get; private set; }

    private Replacement() { }

    public static Replacement Create(Guid performedBy, Guid? oldSealId = null,
        Guid? newSealId = null, Guid? oldCertificateId = null, Guid? newCertificateId = null,
        Guid? revocationId = null)
    {
        return new Replacement
        {
            OldSealId = oldSealId,
            NewSealId = newSealId,
            OldCertificateId = oldCertificateId,
            NewCertificateId = newCertificateId,
            ReplacedAt = DateTimeOffset.UtcNow,
            PerformedBy = performedBy,
            RevocationId = revocationId
        };
    }
}
