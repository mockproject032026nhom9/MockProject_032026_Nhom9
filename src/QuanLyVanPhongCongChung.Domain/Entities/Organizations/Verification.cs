namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;

public class Verification : BaseEntity
{
    public Guid RequestId { get; private set; }
    public string? Result { get; private set; }
    public string? Method { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;

    private Verification() { }

    public static Verification Create(Guid requestId, string? result = null, string? method = null)
    {
        return new Verification
        {
            RequestId = requestId,
            Result = result,
            Method = method
        };
    }
}
