namespace QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotarialAct : BaseAuditableEntity, IAggregateRoot
{
    public Guid RequestId { get; private set; }
    public Guid NotaryId { get; private set; }
    public Guid? JurisdictionId { get; private set; }
    public NotarialActType Type { get; private set; }
    public NotarialActStatus Status { get; private set; }

    public Organizations.ServiceRequest Request { get; private set; } = null!;
    public Notary.Notary Notary { get; private set; } = null!;

    private readonly List<ActSignature> _signatures = [];
    public IReadOnlyCollection<ActSignature> Signatures => _signatures.AsReadOnly();

    private readonly List<ActLogEntry> _logEntries = [];
    public IReadOnlyCollection<ActLogEntry> LogEntries => _logEntries.AsReadOnly();

    private readonly List<ComplianceReview> _complianceReviews = [];
    public IReadOnlyCollection<ComplianceReview> ComplianceReviews => _complianceReviews.AsReadOnly();

    private NotarialAct() { }

    public static NotarialAct Create(Guid requestId, Guid notaryId, NotarialActType type, Guid? jurisdictionId = null)
    {
        return new NotarialAct
        {
            RequestId = requestId,
            NotaryId = notaryId,
            Type = type,
            JurisdictionId = jurisdictionId,
            Status = NotarialActStatus.Draft
        };
    }

    public void UpdateStatus(NotarialActStatus status) => Status = status;
}
