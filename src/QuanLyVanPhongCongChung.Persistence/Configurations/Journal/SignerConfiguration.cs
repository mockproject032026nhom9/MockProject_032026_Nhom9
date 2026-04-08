namespace QuanLyVanPhongCongChung.Persistence.Configurations.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;

public class SignerConfiguration : IEntityTypeConfiguration<Signer>
{
    public void Configure(EntityTypeBuilder<Signer> builder)
    {
        builder.ToTable("Signers");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.JournalEntryId).HasColumnName("journal_entry_id");
        builder.Property(s => s.FullName).HasColumnName("full_name").HasMaxLength(200);

        builder.HasOne(s => s.BiometricData)
            .WithOne(b => b.Signer)
            .HasForeignKey<BiometricData>(b => b.SignerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.JournalEntryId);
    }
}
