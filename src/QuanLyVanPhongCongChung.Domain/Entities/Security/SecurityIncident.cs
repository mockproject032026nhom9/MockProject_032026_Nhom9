namespace QuanLyVanPhongCongChung.Domain.Entities.Security;

using QuanLyVanPhongCongChung.Domain.Common;

public class SecurityIncident : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Guid ReportedBy { get; private set; }
    public DateTimeOffset ReportedAt { get; private set; }
    public Guid? SealId { get; private set; }
    public Guid? CertificateId { get; private set; }

    public Identity.User ReportedByUser { get; private set; } = null!;
    public Seals.Seal? Seal { get; private set; }
    public Seals.Certificate? Certificate { get; private set; }

    private SecurityIncident() { }

    public static SecurityIncident Create(
        string title,
        string description,
        Guid reportedBy,
        DateTimeOffset reportedAt,
        Guid? sealId = null,
        Guid? certificateId = null)
    {
        return new SecurityIncident
        {
            Title = title,
            Description = description,
            ReportedBy = reportedBy,
            ReportedAt = reportedAt,
            SealId = sealId,
            CertificateId = certificateId
        };
    }
}
