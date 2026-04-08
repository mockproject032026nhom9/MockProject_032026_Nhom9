namespace QuanLyVanPhongCongChung.Persistence.Configurations.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Security;

public class SecurityIncidentConfiguration : IEntityTypeConfiguration<SecurityIncident>
{
    public void Configure(EntityTypeBuilder<SecurityIncident> builder)
    {
        builder.ToTable("Incidents");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        builder.Property(i => i.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        builder.Property(i => i.Description).HasColumnName("description").HasMaxLength(2000).IsRequired();
        builder.Property(i => i.ReportedBy).HasColumnName("reported_by");
        builder.Property(i => i.ReportedAt).HasColumnName("reported_at");
        builder.Property(i => i.SealId).HasColumnName("seal_id");
        builder.Property(i => i.CertificateId).HasColumnName("certificate_id");

        builder.HasOne(i => i.ReportedByUser)
            .WithMany()
            .HasForeignKey(i => i.ReportedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Seal)
            .WithMany()
            .HasForeignKey(i => i.SealId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Certificate)
            .WithMany()
            .HasForeignKey(i => i.CertificateId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(i => i.ReportedBy);
        builder.HasIndex(i => i.ReportedAt);
        builder.HasIndex(i => i.SealId);
        builder.HasIndex(i => i.CertificateId);
    }
}
