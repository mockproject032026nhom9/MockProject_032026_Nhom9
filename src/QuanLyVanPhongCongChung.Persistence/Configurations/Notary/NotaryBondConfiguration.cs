namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryBondConfiguration : IEntityTypeConfiguration<NotaryBond>
{
    public void Configure(EntityTypeBuilder<NotaryBond> builder)
    {
        builder.ToTable("NotaryBonds");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("id");
        builder.Property(b => b.NotaryId).HasColumnName("notary_id");
        builder.Property(b => b.ProviderName).HasColumnName("provider_name").HasMaxLength(200).IsRequired();
        builder.Property(b => b.BondAmount).HasColumnName("bond_amount").HasPrecision(18, 2);
        builder.Property(b => b.EffectiveDate).HasColumnName("effective_date");
        builder.Property(b => b.ExpirationDate).HasColumnName("expiration_date");
        builder.Property(b => b.FileUrl).HasColumnName("file_url").HasMaxLength(500);

        builder.HasIndex(b => b.NotaryId);
    }
}
