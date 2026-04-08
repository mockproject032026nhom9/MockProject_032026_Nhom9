namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryCapabilityConfiguration : IEntityTypeConfiguration<NotaryCapability>
{
    public void Configure(EntityTypeBuilder<NotaryCapability> builder)
    {
        builder.ToTable("NotaryCapabilities");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.NotaryId).HasColumnName("notary_id");
        builder.Property(c => c.Mobile).HasColumnName("mobile");
        builder.Property(c => c.Ron).HasColumnName("ron");
        builder.Property(c => c.LoanSigning).HasColumnName("loan_signing");
        builder.Property(c => c.ApostilleRelatedSupport).HasColumnName("apostille_related_support");
        builder.Property(c => c.MaxDistance).HasColumnName("max_distance").HasPrecision(10, 2);

        builder.HasOne(c => c.RonTechnology)
            .WithOne(r => r.Capability)
            .HasForeignKey<RonTechnology>(r => r.CapabilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.NotaryId).IsUnique();
    }
}
