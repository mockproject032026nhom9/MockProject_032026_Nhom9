namespace QuanLyVanPhongCongChung.Persistence.Configurations.Journal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Journal;

public class ExportHistoryConfiguration : IEntityTypeConfiguration<ExportHistory>
{
    public void Configure(EntityTypeBuilder<ExportHistory> builder)
    {
        builder.ToTable("ExportHistories");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.RequestedBy).HasColumnName("requested_by");
        builder.Property(e => e.ExportScope).HasColumnName("export_scope").HasMaxLength(500);
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(e => e.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(e => e.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(e => e.RequestedByUser)
            .WithMany()
            .HasForeignKey(e => e.RequestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.RequestedBy);
    }
}
