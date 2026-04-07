namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;

public class NotaryAvailability : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public int WorkingDaysPerWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public string? FixedDayOff { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryAvailability() { }

    public static NotaryAvailability Create(Guid notaryId, int workingDaysPerWeek,
        TimeOnly startTime, TimeOnly endTime, string? fixedDayOff = null)
    {
        return new NotaryAvailability
        {
            NotaryId = notaryId,
            WorkingDaysPerWeek = workingDaysPerWeek,
            StartTime = startTime,
            EndTime = endTime,
            FixedDayOff = fixedDayOff
        };
    }
}
