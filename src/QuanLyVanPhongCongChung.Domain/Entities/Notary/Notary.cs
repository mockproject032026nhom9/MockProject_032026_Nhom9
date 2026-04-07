namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Notary : BaseAuditableEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string Ssn { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateOnly DateOfBirth { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string? Phone { get; private set; }
    public string? Email { get; private set; }
    public EmploymentType EmploymentType { get; private set; }
    public DateOnly StartDate { get; private set; }
    public string? InternalNotes { get; private set; }
    public NotaryStatus Status { get; private set; }
    public string? ResidentialAddress { get; private set; }

    public User User { get; private set; } = null!;

    private readonly List<NotaryCommission> _commissions = [];
    public IReadOnlyCollection<NotaryCommission> Commissions => _commissions.AsReadOnly();

    private readonly List<NotaryBond> _bonds = [];
    public IReadOnlyCollection<NotaryBond> Bonds => _bonds.AsReadOnly();

    private readonly List<NotaryInsurance> _insurances = [];
    public IReadOnlyCollection<NotaryInsurance> Insurances => _insurances.AsReadOnly();

    private readonly List<NotaryServiceArea> _serviceAreas = [];
    public IReadOnlyCollection<NotaryServiceArea> ServiceAreas => _serviceAreas.AsReadOnly();

    private readonly List<NotaryDocument> _documents = [];
    public IReadOnlyCollection<NotaryDocument> Documents => _documents.AsReadOnly();

    private readonly List<NotaryStatusHistory> _statusHistories = [];
    public IReadOnlyCollection<NotaryStatusHistory> StatusHistories => _statusHistories.AsReadOnly();

    private readonly List<NotaryAuditLog> _auditLogs = [];
    public IReadOnlyCollection<NotaryAuditLog> AuditLogs => _auditLogs.AsReadOnly();

    private readonly List<NotaryIncident> _incidents = [];
    public IReadOnlyCollection<NotaryIncident> Incidents => _incidents.AsReadOnly();

    private NotaryAvailability? _availability = null;
    public NotaryAvailability? Availability => _availability;

    private NotaryCapability? _capability = null;
    public NotaryCapability? Capability => _capability;

    private Notary() { }

    public static Notary Create(Guid userId, string ssn, string fullName, DateOnly dateOfBirth,
        EmploymentType employmentType, DateOnly startDate)
    {
        return new Notary
        {
            UserId = userId,
            Ssn = ssn,
            FullName = fullName,
            DateOfBirth = dateOfBirth,
            EmploymentType = employmentType,
            StartDate = startDate,
            Status = NotaryStatus.Active
        };
    }

    public void UpdateStatus(NotaryStatus newStatus) => Status = newStatus;
    public void SetInternalNotes(string? notes) => InternalNotes = notes;
}
