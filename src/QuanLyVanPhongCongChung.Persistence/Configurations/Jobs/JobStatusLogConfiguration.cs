namespace QuanLyVanPhongCongChung.Persistence.Configurations.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;

public class JobStatusLogConfiguration : IEntityTypeConfiguration<JobStatusLog>
{
    public void Configure(EntityTypeBuilder<JobStatusLog> builder)
    {
        builder.ToTable("JobStatusLogs");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("job_status_id");
        builder.Property(s => s.JobId).HasColumnName("job_id");
        builder.Property(s => s.Status).HasColumnName("status");
        builder.Property(s => s.Timestamp).HasColumnName("time_stamps");
        builder.Property(s => s.Delay).HasColumnName("delay").HasMaxLength(100);
        builder.Property(s => s.ExceptionFlags).HasColumnName("exception_flags").HasMaxLength(200);
        builder.Property(s => s.Note).HasColumnName("note").HasMaxLength(1000);

        builder.HasIndex(s => s.JobId);
    }
}
