namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class DigitalSignatureConfiguration : IEntityTypeConfiguration<DigitalSignature>
{
    public void Configure(EntityTypeBuilder<DigitalSignature> builder)
    {
        builder.ToTable("DigitalSignatures");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.UserId).HasColumnName("user_id");
        builder.Property(d => d.CertificateId).HasColumnName("certificate_id");
        builder.Property(d => d.DeviceId).HasColumnName("device_id");
        builder.Property(d => d.SignatureValue).HasColumnName("signature_value").HasMaxLength(2000).IsRequired();
        builder.Property(d => d.DocumentHash).HasColumnName("document_hash").HasMaxLength(500).IsRequired();
        builder.Property(d => d.SignedAt).HasColumnName("signed_at");
        builder.Property(d => d.IpAddress).HasColumnName("ip_address").HasMaxLength(50);
        builder.Property(d => d.VerificationStatus).HasColumnName("verification_status");

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Certificate)
            .WithMany(c => c.DigitalSignatures)
            .HasForeignKey(d => d.CertificateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Device)
            .WithMany()
            .HasForeignKey(d => d.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(d => d.UserId);
        builder.HasIndex(d => d.CertificateId);
        builder.HasIndex(d => d.DeviceId);
    }
}
