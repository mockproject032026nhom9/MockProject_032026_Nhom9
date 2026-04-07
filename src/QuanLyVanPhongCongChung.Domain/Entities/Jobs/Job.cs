namespace QuanLyVanPhongCongChung.Domain.Entities.Jobs;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;
using QuanLyVanPhongCongChung.Domain.Enums;

public class Job : BaseAuditableEntity, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public ServiceType ServiceType { get; private set; }
    public LocationType LocationType { get; private set; }
    public string? LocationDetails { get; private set; }
    public DateTimeOffset RequestedStartTime { get; private set; }
    public DateTimeOffset RequestedEndTime { get; private set; }
    public int SignerCount { get; private set; }
    public JobStatus Status { get; private set; }

    public User Client { get; private set; } = null!;

    private readonly List<JobAssignment> _assignments = [];
    public IReadOnlyCollection<JobAssignment> Assignments => _assignments.AsReadOnly();

    private readonly List<JobStatusLog> _statusLogs = [];
    public IReadOnlyCollection<JobStatusLog> StatusLogs => _statusLogs.AsReadOnly();

    private readonly List<Notification> _notifications = [];
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    private Job() { }

    public static Job Create(Guid clientId, ServiceType serviceType, LocationType locationType,
        string? locationDetails, DateTimeOffset requestedStartTime, DateTimeOffset requestedEndTime,
        int signerCount)
    {
        return new Job
        {
            ClientId = clientId,
            ServiceType = serviceType,
            LocationType = locationType,
            LocationDetails = locationDetails,
            RequestedStartTime = requestedStartTime,
            RequestedEndTime = requestedEndTime,
            SignerCount = signerCount,
            Status = JobStatus.New
        };
    }

    public void UpdateStatus(JobStatus status) => Status = status;
}
