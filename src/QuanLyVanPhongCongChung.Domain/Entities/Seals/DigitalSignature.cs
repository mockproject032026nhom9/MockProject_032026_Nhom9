namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class DigitalSignature : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid CertificateId { get; private set; }
    public Guid? DeviceId { get; private set; }
    public string SignatureValue { get; private set; } = null!;
    public string DocumentHash { get; private set; } = null!;
    public DateTimeOffset SignedAt { get; private set; }
    public string? IpAddress { get; private set; }
    public VerificationStatusEnum VerificationStatus { get; private set; }

    public Identity.User User { get; private set; } = null!;
    public Certificate Certificate { get; private set; } = null!;
    public Device? Device { get; private set; }

    private DigitalSignature() { }

    public static DigitalSignature Create(Guid userId, Guid certificateId, string signatureValue,
        string documentHash, string? ipAddress = null, Guid? deviceId = null)
    {
        return new DigitalSignature
        {
            UserId = userId,
            CertificateId = certificateId,
            SignatureValue = signatureValue,
            DocumentHash = documentHash,
            SignedAt = DateTimeOffset.UtcNow,
            IpAddress = ipAddress,
            DeviceId = deviceId,
            VerificationStatus = VerificationStatusEnum.Pending
        };
    }
}
