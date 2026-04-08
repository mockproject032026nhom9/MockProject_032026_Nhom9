namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotaryCommission : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public string CommissionState { get; private set; } = null!;
    public string CommissionNumber { get; private set; } = null!;
    public DateOnly IssueDate { get; private set; }
    public DateOnly ExpirationDate { get; private set; }
    public CommissionStatus Status { get; private set; }
    public bool IsRenewalApplied { get; private set; }
    public DateOnly? ExpectedRenewalDate { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private readonly List<AuthorityScope> _authorityScopes = [];
    public IReadOnlyCollection<AuthorityScope> AuthorityScopes => _authorityScopes.AsReadOnly();

    private NotaryCommission() { }

    public static NotaryCommission Create(Guid notaryId, string commissionState, string commissionNumber,
        DateOnly issueDate, DateOnly expirationDate)
    {
        return new NotaryCommission
        {
            NotaryId = notaryId,
            CommissionState = commissionState,
            CommissionNumber = commissionNumber,
            IssueDate = issueDate,
            ExpirationDate = expirationDate,
            Status = CommissionStatus.Valid,
            IsRenewalApplied = false
        };
    }
}
