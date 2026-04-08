namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryInsuranceConfiguration : IEntityTypeConfiguration<NotaryInsurance>
{
    public void Configure(EntityTypeBuilder<NotaryInsurance> builder)
    {
        builder.ToTable("NotaryInsurances");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        builder.Property(i => i.NotaryId).HasColumnName("notary_id");
        builder.Property(i => i.PolicyNumber).HasColumnName("policy_number").HasMaxLength(100).IsRequired();
        builder.Property(i => i.ProviderName).HasColumnName("provider_name").HasMaxLength(200).IsRequired();
        builder.Property(i => i.CoverageAmount).HasColumnName("coverage_amount").HasPrecision(18, 2);
        builder.Property(i => i.EffectiveDate).HasColumnName("effective_date");
        builder.Property(i => i.ExpirationDate).HasColumnName("expiration_date");
        builder.Property(i => i.FileUrl).HasColumnName("file_url").HasMaxLength(500);

        builder.HasIndex(i => i.NotaryId);
    }
}
