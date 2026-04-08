namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryStatusHistoryConfiguration : IEntityTypeConfiguration<NotaryStatusHistory>
{
    public void Configure(EntityTypeBuilder<NotaryStatusHistory> builder)
    {
        builder.ToTable("NotaryStatusHistory");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.NotaryId).HasColumnName("notary_id");
        builder.Property(s => s.Status).HasColumnName("status");
        builder.Property(s => s.Reason).HasColumnName("reason").HasMaxLength(500);
        builder.Property(s => s.EffectiveDate).HasColumnName("effective_date");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(100);

        builder.HasIndex(s => s.NotaryId);
    }
}
