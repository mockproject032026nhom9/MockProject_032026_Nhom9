namespace QuanLyVanPhongCongChung.Domain.Entities.Security;

using QuanLyVanPhongCongChung.Domain.Common;

public class Revocation : BaseEntity
{
    public Guid? SealId { get; private set; }
    public Guid? CertificateId { get; private set; }
    public string Reason { get; private set; } = null!;
    public DateOnly EffectiveDate { get; private set; }
    public Guid PerformedBy { get; private set; }
    public Guid? IncidentId { get; private set; }

    public Seals.Seal? Seal { get; private set; }
    public Seals.Certificate? Certificate { get; private set; }
    public Identity.User PerformedByUser { get; private set; } = null!;
    public Notary.NotaryIncident? Incident { get; private set; }

    private Revocation() { }

    public static Revocation Create(string reason, DateOnly effectiveDate, Guid performedBy,
        Guid? sealId = null, Guid? certificateId = null, Guid? incidentId = null)
    {
        return new Revocation
        {
            SealId = sealId,
            CertificateId = certificateId,
            Reason = reason,
            EffectiveDate = effectiveDate,
            PerformedBy = performedBy,
            IncidentId = incidentId
        };
    }
}
