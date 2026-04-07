namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Payment : BaseEntity
{
    public Guid RequestId { get; private set; }
    public decimal Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? Gateway { get; private set; }
    public string? Transaction { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;

    private Payment() { }

    public static Payment Create(Guid requestId, decimal amount, string? gateway = null)
    {
        return new Payment
        {
            RequestId = requestId,
            Amount = amount,
            Status = PaymentStatus.Pending,
            Gateway = gateway
        };
    }
}
