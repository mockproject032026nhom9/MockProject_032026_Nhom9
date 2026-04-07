namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;

public class RonTechnology : BaseEntity
{
    public Guid CapabilityId { get; private set; }
    public bool RonCameraReady { get; private set; }
    public bool RonInternetReady { get; private set; }
    public string? DigitalStatus { get; private set; }

    public NotaryCapability Capability { get; private set; } = null!;

    private RonTechnology() { }

    public static RonTechnology Create(Guid capabilityId, bool ronCameraReady,
        bool ronInternetReady, string? digitalStatus = null)
    {
        return new RonTechnology
        {
            CapabilityId = capabilityId,
            RonCameraReady = ronCameraReady,
            RonInternetReady = ronInternetReady,
            DigitalStatus = digitalStatus
        };
    }
}
