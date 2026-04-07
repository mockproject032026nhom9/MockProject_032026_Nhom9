namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class AuthorityScope : BaseEntity
{
    public Guid CommissionId { get; private set; }
    public AuthorityType AuthorityType { get; private set; }

    public NotaryCommission Commission { get; private set; } = null!;

    private AuthorityScope() { }

    public static AuthorityScope Create(Guid commissionId, AuthorityType authorityType)
    {
        return new AuthorityScope
        {
            CommissionId = commissionId,
            AuthorityType = authorityType
        };
    }
}
