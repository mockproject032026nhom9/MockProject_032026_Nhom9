namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
{
    public void Configure(EntityTypeBuilder<Certificate> builder)
    {
        builder.ToTable("certificates");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.OwnerUserId).HasColumnName("owner_user_id");
        builder.Property(c => c.CaId).HasColumnName("ca_id");
        builder.Property(c => c.HsmKeyId).HasColumnName("hsm_key_id");
        builder.Property(c => c.SerialNumber).HasColumnName("serial_number").HasMaxLength(200).IsRequired();
        builder.Property(c => c.PublicKey).HasColumnName("public_key").HasMaxLength(4000).IsRequired();
        builder.Property(c => c.Thumbprint).HasColumnName("thumbprint").HasMaxLength(200).IsRequired();
        builder.Property(c => c.Algorithm).HasColumnName("algorithm").HasMaxLength(50).IsRequired();
        builder.Property(c => c.ValidFrom).HasColumnName("valid_from");
        builder.Property(c => c.ValidTo).HasColumnName("valid_to");
        builder.Property(c => c.Status).HasColumnName("status");
        builder.Property(c => c.ReplaceCertId).HasColumnName("replace_cert_id");
        builder.Property(c => c.DeviceId).HasColumnName("device_id");
        builder.Property(c => c.CreatedAt).HasColumnName("created_at");
        builder.Property(c => c.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(c => c.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(c => c.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(c => c.OwnerUser)
            .WithMany()
            .HasForeignKey(c => c.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Ca)
            .WithMany(ca => ca.Certificates)
            .HasForeignKey(c => c.CaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.HsmKey)
            .WithMany()
            .HasForeignKey(c => c.HsmKeyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Device)
            .WithMany()
            .HasForeignKey(c => c.DeviceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.ReplaceCert)
            .WithMany()
            .HasForeignKey(c => c.ReplaceCertId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.DigitalSignatures)
            .WithOne(d => d.Certificate)
            .HasForeignKey(d => d.CertificateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.OwnerUserId);
        builder.HasIndex(c => c.CaId);
        builder.HasIndex(c => c.HsmKeyId);
        builder.HasIndex(c => c.DeviceId);
        builder.HasIndex(c => c.ReplaceCertId);
        builder.HasIndex(c => c.Status);

        builder.Navigation(c => c.DigitalSignatures).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
