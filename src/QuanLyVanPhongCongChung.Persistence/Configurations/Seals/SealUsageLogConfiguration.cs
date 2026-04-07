namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class SealUsageLogConfiguration : IEntityTypeConfiguration<SealUsageLog>
{
    public void Configure(EntityTypeBuilder<SealUsageLog> builder)
    {
        builder.ToTable("seal_usage_logs");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.SealId).HasColumnName("seal_id");
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.Property(s => s.UsedAt).HasColumnName("used_at");
        builder.Property(s => s.PageNumber).HasColumnName("page_number");
        builder.Property(s => s.IsAnomaly).HasColumnName("is_anomaly");

        builder.HasOne(s => s.Seal)
            .WithMany(seal => seal.UsageLogs)
            .HasForeignKey(s => s.SealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.SealId);
        builder.HasIndex(s => s.UserId);
    }
}
