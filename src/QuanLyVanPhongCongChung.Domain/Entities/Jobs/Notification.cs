namespace QuanLyVanPhongCongChung.Domain.Entities.Jobs;

using QuanLyVanPhongCongChung.Domain.Common;

public class Notification : BaseEntity
{
    public Guid EventId { get; private set; }
    public Guid JobId { get; private set; }
    public bool Sms { get; private set; }
    public bool Email { get; private set; }
    public bool App { get; private set; }
    public string? Delay { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public string? Content { get; private set; }

    public Event Event { get; private set; } = null!;
    public Job Job { get; private set; } = null!;

    private Notification() { }

    public static Notification Create(Guid eventId, Guid jobId, string? content,
        bool sms = false, bool email = false, bool app = false, string? delay = null)
    {
        return new Notification
        {
            EventId = eventId,
            JobId = jobId,
            Content = content,
            Sms = sms,
            Email = email,
            App = app,
            Delay = delay,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}
