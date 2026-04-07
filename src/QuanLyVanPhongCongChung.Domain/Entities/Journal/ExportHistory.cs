namespace QuanLyVanPhongCongChung.Domain.Entities.Journal;

using QuanLyVanPhongCongChung.Domain.Common;

public class ExportHistory : BaseAuditableEntity
{
    public Guid RequestedBy { get; private set; }
    public string? ExportScope { get; private set; }

    public Identity.User RequestedByUser { get; private set; } = null!;

    private ExportHistory() { }

    public static ExportHistory Create(Guid requestedBy, string? exportScope = null)
    {
        return new ExportHistory
        {
            RequestedBy = requestedBy,
            ExportScope = exportScope
        };
    }
}
