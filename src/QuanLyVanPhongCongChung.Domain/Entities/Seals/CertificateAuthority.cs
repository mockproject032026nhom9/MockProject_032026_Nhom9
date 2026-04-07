namespace QuanLyVanPhongCongChung.Domain.Entities.Seals;

using QuanLyVanPhongCongChung.Domain.Common;

public class CertificateAuthority : BaseEntity
{
    public string Name { get; private set; } = null!;
    public bool IsApproved { get; private set; }

    private readonly List<Certificate> _certificates = [];
    public IReadOnlyCollection<Certificate> Certificates => _certificates.AsReadOnly();

    private CertificateAuthority() { }

    public static CertificateAuthority Create(string name, bool isApproved = false)
    {
        return new CertificateAuthority
        {
            Name = name,
            IsApproved = isApproved
        };
    }
}
