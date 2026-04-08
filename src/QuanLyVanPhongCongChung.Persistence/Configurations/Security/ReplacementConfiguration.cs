namespace QuanLyVanPhongCongChung.Persistence.Configurations.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Security;

public class ReplacementConfiguration : IEntityTypeConfiguration<Replacement>
{
    public void Configure(EntityTypeBuilder<Replacement> builder)
    {
        builder.ToTable("Replacements");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.OldSealId).HasColumnName("old_seal_id");
        builder.Property(r => r.OldCertificateId).HasColumnName("old_certificate_id");
        builder.Property(r => r.NewSealId).HasColumnName("new_seal_id");
        builder.Property(r => r.NewCertificateId).HasColumnName("new_certificate_id");
        builder.Property(r => r.ReplacedAt).HasColumnName("replaced_at");
        builder.Property(r => r.PerformedBy).HasColumnName("performed_by");
        builder.Property(r => r.RevocationId).HasColumnName("revocation_id");

        builder.HasOne(r => r.OldSeal)
            .WithMany()
            .HasForeignKey(r => r.OldSealId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.NewSeal)
            .WithMany()
            .HasForeignKey(r => r.NewSealId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.OldCertificate)
            .WithMany()
            .HasForeignKey(r => r.OldCertificateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.NewCertificate)
            .WithMany()
            .HasForeignKey(r => r.NewCertificateId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(r => r.PerformedByUser)
            .WithMany()
            .HasForeignKey(r => r.PerformedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Revocation)
            .WithMany()
            .HasForeignKey(r => r.RevocationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(r => r.OldSealId);
        builder.HasIndex(r => r.NewSealId);
        builder.HasIndex(r => r.OldCertificateId);
        builder.HasIndex(r => r.NewCertificateId);
        builder.HasIndex(r => r.PerformedBy);
    }
}
