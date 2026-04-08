namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class ServiceRequest : BaseAuditableEntity, IAggregateRoot
{
    public Guid? CustomerId { get; private set; }
    public Guid? OrganizationId { get; private set; }
    public RequestStatus Status { get; private set; }

    public Identity.User? Customer { get; private set; }
    public Organization? Organization { get; private set; }

    private Verification? _verification = null;
    public Verification? Verification => _verification;

    private Delivery? _delivery = null;
    public Delivery? Delivery => _delivery;

    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    private readonly List<Document> _documents = [];
    public IReadOnlyCollection<Document> Documents => _documents.AsReadOnly();

    private ServiceRequest() { }

    public static ServiceRequest Create(RequestStatus status, Guid? customerId = null, Guid? organizationId = null)
    {
        return new ServiceRequest
        {
            CustomerId = customerId,
            OrganizationId = organizationId,
            Status = status
        };
    }

    public void UpdateStatus(RequestStatus status) => Status = status;
}
