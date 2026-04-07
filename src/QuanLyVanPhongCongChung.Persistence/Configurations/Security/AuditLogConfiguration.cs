namespace QuanLyVanPhongCongChung.Persistence.Configurations.Security;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Security;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.UserId).HasColumnName("user_id");
        builder.Property(a => a.Action).HasColumnName("action").HasMaxLength(200).IsRequired();
        builder.Property(a => a.EntityType).HasColumnName("entity_type").HasMaxLength(200).IsRequired();
        builder.Property(a => a.EntityId).HasColumnName("entity_id");
        builder.Property(a => a.Timestamp).HasColumnName("timestamp");
        builder.Property(a => a.Metadata).HasColumnName("metadata");

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => new { a.EntityType, a.EntityId });
        builder.HasIndex(a => a.Timestamp);
    }
}
