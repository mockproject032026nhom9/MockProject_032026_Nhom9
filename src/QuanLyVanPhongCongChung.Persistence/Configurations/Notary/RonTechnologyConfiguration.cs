namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class RonTechnologyConfiguration : IEntityTypeConfiguration<RonTechnology>
{
    public void Configure(EntityTypeBuilder<RonTechnology> builder)
    {
        builder.ToTable("RonTechnologies");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.CapabilityId).HasColumnName("capability_id");
        builder.Property(r => r.RonCameraReady).HasColumnName("ron_camera_ready");
        builder.Property(r => r.RonInternetReady).HasColumnName("ron_internet_ready");
        builder.Property(r => r.DigitalStatus).HasColumnName("digital_status").HasMaxLength(100);

        builder.HasIndex(r => r.CapabilityId).IsUnique();
    }
}
