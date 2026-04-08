namespace QuanLyVanPhongCongChung.Persistence.Configurations.NotarialActs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

public class ComplianceReviewConfiguration : IEntityTypeConfiguration<ComplianceReview>
{
    public void Configure(EntityTypeBuilder<ComplianceReview> builder)
    {
        builder.ToTable("ComplianceReviews");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.ActId).HasColumnName("act_id");
        builder.Property(r => r.Result).HasColumnName("result").HasMaxLength(2000);

        builder.HasIndex(r => r.ActId);
    }
}
