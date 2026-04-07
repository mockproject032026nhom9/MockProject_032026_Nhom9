namespace QuanLyVanPhongCongChung.Persistence.Configurations.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnName("id");
        builder.Property(n => n.EventId).HasColumnName("event_id");
        builder.Property(n => n.JobId).HasColumnName("job_id");
        builder.Property(n => n.Sms).HasColumnName("sms");
        builder.Property(n => n.Email).HasColumnName("email");
        builder.Property(n => n.App).HasColumnName("app");
        builder.Property(n => n.Delay).HasColumnName("delay").HasMaxLength(50);
        builder.Property(n => n.Timestamp).HasColumnName("timestamp");
        builder.Property(n => n.Content).HasColumnName("content").HasMaxLength(2000);

        builder.HasOne(n => n.Event)
            .WithMany(e => e.Notifications)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(n => n.Job)
            .WithMany(j => j.Notifications)
            .HasForeignKey(n => n.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(n => n.EventId);
        builder.HasIndex(n => n.JobId);
    }
}
