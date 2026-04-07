namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryCommissionConfiguration : IEntityTypeConfiguration<NotaryCommission>
{
    public void Configure(EntityTypeBuilder<NotaryCommission> builder)
    {
        builder.ToTable("notary_commissions");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");
        builder.Property(c => c.NotaryId).HasColumnName("notary_id");
        builder.Property(c => c.CommissionStateId).HasColumnName("commission_state_id");
        builder.Property(c => c.CommissionNumber).HasColumnName("commission_number").HasMaxLength(100).IsRequired();
        builder.Property(c => c.IssueDate).HasColumnName("issue_date");
        builder.Property(c => c.ExpirationDate).HasColumnName("expiration_date");
        builder.Property(c => c.Status).HasColumnName("status");
        builder.Property(c => c.IsRenewalApplied).HasColumnName("is_renewal_applied");
        builder.Property(c => c.ExpectedRenewalDate).HasColumnName("expected_renewal_date");

        builder.HasOne(c => c.CommissionState)
            .WithMany()
            .HasForeignKey(c => c.CommissionStateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.AuthorityScopes)
            .WithOne(a => a.Commission)
            .HasForeignKey(a => a.CommissionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.NotaryId, c.Status });
        builder.HasIndex(c => c.ExpirationDate);

        builder.Navigation(c => c.AuthorityScopes).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
