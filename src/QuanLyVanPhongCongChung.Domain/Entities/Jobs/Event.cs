namespace QuanLyVanPhongCongChung.Domain.Entities.Jobs;

using QuanLyVanPhongCongChung.Domain.Common;

public class Event : BaseEntity
{
    public string EventName { get; private set; } = null!;

    private readonly List<Notification> _notifications = [];
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    private Event() { }

    public static Event Create(string eventName)
    {
        return new Event { EventName = eventName };
    }
}
