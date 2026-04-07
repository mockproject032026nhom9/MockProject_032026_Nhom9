namespace QuanLyVanPhongCongChung.Domain.Entities.Journal;

using QuanLyVanPhongCongChung.Domain.Common;

public class BiometricData : BaseEntity
{
    public Guid SignerId { get; private set; }
    public string? SignatureImage { get; private set; }

    public Signer Signer { get; private set; } = null!;

    private BiometricData() { }

    public static BiometricData Create(Guid signerId, string? signatureImage)
    {
        return new BiometricData
        {
            SignerId = signerId,
            SignatureImage = signatureImage
        };
    }
}
