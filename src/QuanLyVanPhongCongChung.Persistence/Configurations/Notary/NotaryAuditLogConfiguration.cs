namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryAuditLogConfiguration : IEntityTypeConfiguration<NotaryAuditLog>
{
    public void Configure(EntityTypeBuilder<NotaryAuditLog> builder)
    {
        builder.ToTable("NotaryAuditLogs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.NotaryId).HasColumnName("notary_id");
        builder.Property(a => a.TableName).HasColumnName("table_name").HasMaxLength(100).IsRequired();
        builder.Property(a => a.RecordId).HasColumnName("record_id");
        builder.Property(a => a.Action).HasColumnName("action");
        builder.Property(a => a.OldValue).HasColumnName("old_value");
        builder.Property(a => a.NewValue).HasColumnName("new_value");
        builder.Property(a => a.ChangedBy).HasColumnName("changed_by").HasMaxLength(100);
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(a => new { a.NotaryId, a.CreatedAt });
    }
}
