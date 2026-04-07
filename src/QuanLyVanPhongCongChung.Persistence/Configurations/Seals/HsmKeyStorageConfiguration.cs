namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class HsmKeyStorageConfiguration : IEntityTypeConfiguration<HsmKeyStorage>
{
    public void Configure(EntityTypeBuilder<HsmKeyStorage> builder)
    {
        builder.ToTable("hsm_key_storages");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).HasColumnName("id");
        builder.Property(h => h.ProviderName).HasColumnName("provider_name").HasMaxLength(200).IsRequired();
        builder.Property(h => h.HsmSerialNumber).HasColumnName("hsm_serial_number").HasMaxLength(200).IsRequired();
        builder.Property(h => h.KeyRotationStatus).HasColumnName("key_rotation_status");
        builder.Property(h => h.LastRotationAt).HasColumnName("last_rotation_at");
        builder.Property(h => h.NextRotationDue).HasColumnName("next_rotation_due");
    }
}
