namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Delivery : BaseEntity
{
    public Guid RequestId { get; private set; }
    public DeliveryMethod Method { get; private set; }
    public DeliveryStatus Status { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;

    private Delivery() { }

    public static Delivery Create(Guid requestId, DeliveryMethod method)
    {
        return new Delivery
        {
            RequestId = requestId,
            Method = method,
            Status = DeliveryStatus.Pending
        };
    }
}
