namespace QuanLyVanPhongCongChung.Persistence.Configurations.NotarialActs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

public class ActLogEntryConfiguration : IEntityTypeConfiguration<ActLogEntry>
{
    public void Configure(EntityTypeBuilder<ActLogEntry> builder)
    {
        builder.ToTable("act_log_entries");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id");
        builder.Property(l => l.ActId).HasColumnName("act_id");
        builder.Property(l => l.NotaryId).HasColumnName("notary_id");
        builder.Property(l => l.Timestamp).HasColumnName("timestamp");

        builder.HasOne(l => l.Notary)
            .WithMany()
            .HasForeignKey(l => l.NotaryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.ActId);
        builder.HasIndex(l => l.NotaryId);
    }
}
