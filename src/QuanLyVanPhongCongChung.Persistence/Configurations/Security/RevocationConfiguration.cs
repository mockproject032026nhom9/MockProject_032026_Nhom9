namespace QuanLyVanPhongCongChung.Persistence.Configurations.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Security;

public class RevocationConfiguration : IEntityTypeConfiguration<Revocation>
{
    public void Configure(EntityTypeBuilder<Revocation> builder)
    {
        builder.ToTable("revocations");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.SealId).HasColumnName("seal_id");
        builder.Property(r => r.CertificateId).HasColumnName("certificate_id");
        builder.Property(r => r.Reason).HasColumnName("reason").HasMaxLength(1000).IsRequired();
        builder.Property(r => r.EffectiveDate).HasColumnName("effective_date");
        builder.Property(r => r.PerformedBy).HasColumnName("performed_by");
        builder.Property(r => r.IncidentId).HasColumnName("incident_id");

        builder.HasOne(r => r.Seal)
            .WithMany()
            .HasForeignKey(r => r.SealId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.Certificate)
            .WithMany()
            .HasForeignKey(r => r.CertificateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.PerformedByUser)
            .WithMany()
            .HasForeignKey(r => r.PerformedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Incident)
            .WithMany()
            .HasForeignKey(r => r.IncidentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(r => r.SealId);
        builder.HasIndex(r => r.CertificateId);
        builder.HasIndex(r => r.PerformedBy);
    }
}
