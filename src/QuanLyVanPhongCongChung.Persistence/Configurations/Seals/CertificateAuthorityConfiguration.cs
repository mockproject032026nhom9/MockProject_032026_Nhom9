namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class CertificateAuthorityConfiguration : IEntityTypeConfiguration<CertificateAuthority>
{
    public void Configure(EntityTypeBuilder<CertificateAuthority> builder)
    {
        builder.ToTable("CertificateAuthorities");
        builder.HasKey(ca => ca.Id);
        builder.Property(ca => ca.Id).HasColumnName("id");
        builder.Property(ca => ca.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(ca => ca.IsApproved).HasColumnName("is_approved");

        builder.HasMany(ca => ca.Certificates)
            .WithOne(c => c.Ca)
            .HasForeignKey(c => c.CaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(ca => ca.Certificates).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
