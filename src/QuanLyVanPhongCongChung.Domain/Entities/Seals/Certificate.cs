namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Certificate : BaseAuditableEntity, IAggregateRoot
{
    public Guid OwnerUserId { get; private set; }
    public Guid CaId { get; private set; }
    public Guid? HsmKeyId { get; private set; }
    public string SerialNumber { get; private set; } = null!;
    public string PublicKey { get; private set; } = null!;
    public string Thumbprint { get; private set; } = null!;
    public string Algorithm { get; private set; } = null!;
    public DateTimeOffset ValidFrom { get; private set; }
    public DateTimeOffset ValidTo { get; private set; }
    public CertificateStatus Status { get; private set; }
    public Guid? ReplaceCertId { get; private set; }
    public Guid? DeviceId { get; private set; }

    public Identity.User OwnerUser { get; private set; } = null!;
    public CertificateAuthority Ca { get; private set; } = null!;
    public HsmKeyStorage? HsmKey { get; private set; }
    public Device? Device { get; private set; }
    public Certificate? ReplaceCert { get; private set; }

    private readonly List<DigitalSignature> _digitalSignatures = [];
    public IReadOnlyCollection<DigitalSignature> DigitalSignatures => _digitalSignatures.AsReadOnly();

    private Certificate() { }

    public static Certificate Create(Guid ownerUserId, Guid caId, string serialNumber,
        string publicKey, string thumbprint, string algorithm,
        DateTimeOffset validFrom, DateTimeOffset validTo)
    {
        return new Certificate
        {
            OwnerUserId = ownerUserId,
            CaId = caId,
            SerialNumber = serialNumber,
            PublicKey = publicKey,
            Thumbprint = thumbprint,
            Algorithm = algorithm,
            ValidFrom = validFrom,
            ValidTo = validTo,
            Status = CertificateStatus.Active
        };
    }

    public void Revoke() => Status = CertificateStatus.Revoked;
}
