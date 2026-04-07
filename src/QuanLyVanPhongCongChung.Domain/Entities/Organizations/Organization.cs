namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;

public class Organization : BaseAuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? TaxCode { get; private set; }

    private readonly List<ServiceRequest> _requests = [];
    public IReadOnlyCollection<ServiceRequest> Requests => _requests.AsReadOnly();

    private Organization() { }

    public static Organization Create(string name, string? taxCode = null)
    {
        return new Organization
        {
            Name = name,
            TaxCode = taxCode
        };
    }
}
