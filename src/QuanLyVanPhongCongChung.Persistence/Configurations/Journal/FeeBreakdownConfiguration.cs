namespace QuanLyVanPhongCongChung.Persistence.Configurations.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;

public class FeeBreakdownConfiguration : IEntityTypeConfiguration<FeeBreakdown>
{
    public void Configure(EntityTypeBuilder<FeeBreakdown> builder)
    {
        builder.ToTable("FeeBreakdowns");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasColumnName("id");
        builder.Property(f => f.JournalEntryId).HasColumnName("journal_entry_id");
        builder.Property(f => f.BaseNotarialFee).HasColumnName("base_notarial_fee").HasPrecision(18, 2);
        builder.Property(f => f.ServiceFee).HasColumnName("service_fee").HasPrecision(18, 2);
        builder.Property(f => f.TravelFee).HasColumnName("travel_fee").HasPrecision(18, 2);
        builder.Property(f => f.ConvenienceFee).HasColumnName("convenience_fee").HasPrecision(18, 2);
        builder.Property(f => f.RushFee).HasColumnName("rush_fee").HasPrecision(18, 2);
        builder.Property(f => f.TotalAmount).HasColumnName("total_amount").HasPrecision(18, 2);
        builder.Property(f => f.NotaryShare).HasColumnName("notary_share").HasPrecision(18, 2);
        builder.Property(f => f.CompanyShare).HasColumnName("company_share").HasPrecision(18, 2);

        builder.HasIndex(f => f.JournalEntryId).IsUnique();
    }
}
