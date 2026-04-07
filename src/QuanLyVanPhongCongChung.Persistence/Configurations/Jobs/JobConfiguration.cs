namespace QuanLyVanPhongCongChung.Persistence.Configurations.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasColumnName("id");
        builder.Property(j => j.ClientId).HasColumnName("client_id");
        builder.Property(j => j.ServiceType).HasColumnName("service_type");
        builder.Property(j => j.LocationType).HasColumnName("location_type");
        builder.Property(j => j.LocationDetails).HasColumnName("location_details").HasMaxLength(500);
        builder.Property(j => j.RequestedStartTime).HasColumnName("requested_start_time");
        builder.Property(j => j.RequestedEndTime).HasColumnName("requested_end_time");
        builder.Property(j => j.SignerCount).HasColumnName("signer_count");
        builder.Property(j => j.Status).HasColumnName("status");
        builder.Property(j => j.CreatedAt).HasColumnName("created_at");
        builder.Property(j => j.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(j => j.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(j => j.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(j => j.Client)
            .WithMany()
            .HasForeignKey(j => j.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.Assignments)
            .WithOne(a => a.Job)
            .HasForeignKey(a => a.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(j => j.StatusLogs)
            .WithOne(s => s.Job)
            .HasForeignKey(s => s.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(j => j.Notifications)
            .WithOne(n => n.Job)
            .HasForeignKey(n => n.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(j => j.ClientId);
        builder.HasIndex(j => j.Status);

        builder.Navigation(j => j.Assignments).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(j => j.StatusLogs).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(j => j.Notifications).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
