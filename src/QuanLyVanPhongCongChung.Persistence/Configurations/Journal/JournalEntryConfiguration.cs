namespace QuanLyVanPhongCongChung.Persistence.Configurations.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;

public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.ToTable("journal_entries");
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Id).HasColumnName("id");
        builder.Property(j => j.NotaryId).HasColumnName("notary_id");
        builder.Property(j => j.NotarialFee).HasColumnName("notarial_fee").HasPrecision(18, 2);
        builder.Property(j => j.Status).HasColumnName("status");
        builder.Property(j => j.CreatedAt).HasColumnName("created_at");
        builder.Property(j => j.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(j => j.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(j => j.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(j => j.Notary)
            .WithMany()
            .HasForeignKey(j => j.NotaryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(j => j.Signers)
            .WithOne(s => s.JournalEntry)
            .HasForeignKey(s => s.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(j => j.FeeBreakdown)
            .WithOne(f => f.JournalEntry)
            .HasForeignKey<FeeBreakdown>(f => f.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(j => j.NotaryId);
        builder.HasIndex(j => j.Status);

        builder.Navigation(j => j.Signers).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
