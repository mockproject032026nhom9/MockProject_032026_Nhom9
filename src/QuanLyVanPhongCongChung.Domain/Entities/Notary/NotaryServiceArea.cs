namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;

public class NotaryServiceArea : BaseEntity
{
    public Guid StateId { get; private set; }
    public string? CountyName { get; private set; }
    public Guid NotaryId { get; private set; }

    public State State { get; private set; } = null!;
    public Notary Notary { get; private set; } = null!;

    private NotaryServiceArea() { }

    public static NotaryServiceArea Create(Guid notaryId, Guid stateId, string? countyName = null)
    {
        return new NotaryServiceArea
        {
            NotaryId = notaryId,
            StateId = stateId,
            CountyName = countyName
        };
    }
}
