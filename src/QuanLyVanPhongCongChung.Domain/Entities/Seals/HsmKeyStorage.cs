namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class HsmKeyStorage : BaseEntity
{
    public string ProviderName { get; private set; } = null!;
    public string HsmSerialNumber { get; private set; } = null!;
    public KeyRotationStatus KeyRotationStatus { get; private set; }
    public DateTimeOffset? LastRotationAt { get; private set; }
    public DateTimeOffset? NextRotationDue { get; private set; }

    private HsmKeyStorage() { }

    public static HsmKeyStorage Create(string providerName, string hsmSerialNumber)
    {
        return new HsmKeyStorage
        {
            ProviderName = providerName,
            HsmSerialNumber = hsmSerialNumber,
            KeyRotationStatus = KeyRotationStatus.Current
        };
    }
}
