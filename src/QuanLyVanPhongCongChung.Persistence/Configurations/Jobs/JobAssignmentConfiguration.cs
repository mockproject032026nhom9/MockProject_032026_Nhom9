namespace QuanLyVanPhongCongChung.Persistence.Configurations.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;

public class JobAssignmentConfiguration : IEntityTypeConfiguration<JobAssignment>
{
    public void Configure(EntityTypeBuilder<JobAssignment> builder)
    {
        builder.ToTable("job_assignments");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.JobId).HasColumnName("job_id");
        builder.Property(a => a.NotaryId).HasColumnName("notary_id");
        builder.Property(a => a.AssignedAt).HasColumnName("assigned_at");
        builder.Property(a => a.AcceptedAt).HasColumnName("accepted_at");
        builder.Property(a => a.Status).HasColumnName("status");

        builder.HasOne(a => a.Notary)
            .WithMany()
            .HasForeignKey(a => a.NotaryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.NotaryId, a.Status });
    }
}
