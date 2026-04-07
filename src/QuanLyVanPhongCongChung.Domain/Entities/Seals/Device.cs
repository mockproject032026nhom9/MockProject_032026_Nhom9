namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Device : BaseEntity
{
    public Guid UserId { get; private set; }
    public string DeviceType { get; private set; } = null!;
    public string DeviceIdentifier { get; private set; } = null!;
    public DeviceStatus Status { get; private set; }
    public bool MfaEnabled { get; private set; }

    public Identity.User User { get; private set; } = null!;

    private Device() { }

    public static Device Create(Guid userId, string deviceType, string deviceIdentifier, bool mfaEnabled = false)
    {
        return new Device
        {
            UserId = userId,
            DeviceType = deviceType,
            DeviceIdentifier = deviceIdentifier,
            Status = DeviceStatus.Active,
            MfaEnabled = mfaEnabled
        };
    }
}
