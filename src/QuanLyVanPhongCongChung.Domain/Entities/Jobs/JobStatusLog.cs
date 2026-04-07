namespace QuanLyVanPhongCongChung.Domain.Entities.Jobs;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class JobStatusLog : BaseEntity
{
    public Guid JobId { get; private set; }
    public JobStatus Status { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public string? Delay { get; private set; }
    public string? ExceptionFlags { get; private set; }
    public string? Note { get; private set; }

    public Job Job { get; private set; } = null!;

    private JobStatusLog() { }

    public static JobStatusLog Create(Guid jobId, JobStatus status, string? note = null)
    {
        return new JobStatusLog
        {
            JobId = jobId,
            Status = status,
            Timestamp = DateTimeOffset.UtcNow,
            Note = note
        };
    }
}
