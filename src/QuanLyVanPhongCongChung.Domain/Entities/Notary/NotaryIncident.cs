namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotaryIncident : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public string IncidentType { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public SeverityLevel Severity { get; private set; }
    public IncidentStatus Status { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryIncident() { }

    public static NotaryIncident Create(Guid notaryId, string incidentType,
        string description, SeverityLevel severity)
    {
        return new NotaryIncident
        {
            NotaryId = notaryId,
            IncidentType = incidentType,
            Description = description,
            Severity = severity,
            Status = IncidentStatus.Open
        };
    }

    public void Resolve() { Status = IncidentStatus.Resolved; ResolvedAt = DateTimeOffset.UtcNow; }
}
