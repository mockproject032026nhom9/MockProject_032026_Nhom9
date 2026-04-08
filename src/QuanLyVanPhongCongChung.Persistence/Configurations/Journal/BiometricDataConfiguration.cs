namespace QuanLyVanPhongCongChung.Persistence.Configurations.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;

public class BiometricDataConfiguration : IEntityTypeConfiguration<BiometricData>
{
    public void Configure(EntityTypeBuilder<BiometricData> builder)
    {
        builder.ToTable("BiometricData");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.SignerId).HasColumnName("signer_id");
        builder.Property(b => b.SignatureImage).HasColumnName("signature_image").HasMaxLength(2000);

        builder.HasIndex(b => b.SignerId).IsUnique();
    }
}
