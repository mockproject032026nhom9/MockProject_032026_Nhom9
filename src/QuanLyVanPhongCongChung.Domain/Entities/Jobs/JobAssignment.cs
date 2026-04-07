namespace QuanLyVanPhongCongChung.Domain.Entities.Jobs;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class JobAssignment : BaseEntity
{
    public Guid JobId { get; private set; }
    public Guid NotaryId { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }
    public DateTimeOffset? AcceptedAt { get; private set; }
    public JobAssignmentStatus Status { get; private set; }

    public Job Job { get; private set; } = null!;
    public Notary.Notary Notary { get; private set; } = null!;

    private JobAssignment() { }

    public static JobAssignment Create(Guid jobId, Guid notaryId)
    {
        return new JobAssignment
        {
            JobId = jobId,
            NotaryId = notaryId,
            AssignedAt = DateTimeOffset.UtcNow,
            Status = JobAssignmentStatus.Pending
        };
    }

    public void Accept()
    {
        Status = JobAssignmentStatus.Accepted;
        AcceptedAt = DateTimeOffset.UtcNow;
    }
}
