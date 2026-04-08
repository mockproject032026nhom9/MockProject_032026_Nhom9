namespace QuanLyVanPhongCongChung.Persistence.Configurations.NotarialActs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

public class NotarialActConfiguration : IEntityTypeConfiguration<NotarialAct>
{
    public void Configure(EntityTypeBuilder<NotarialAct> builder)
    {
        builder.ToTable("NotarialActs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.RequestId).HasColumnName("request_id");
        builder.Property(a => a.NotaryId).HasColumnName("notary_id");
        builder.Property(a => a.JurisdictionId).HasColumnName("jurisdiction_id");
        builder.Property(a => a.Type).HasColumnName("type");
        builder.Property(a => a.Status).HasColumnName("status");
        builder.Property(a => a.CreatedAt).HasColumnName("created_at");
        builder.Property(a => a.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(a => a.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(a => a.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(a => a.Request)
            .WithMany()
            .HasForeignKey(a => a.RequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Notary)
            .WithMany()
            .HasForeignKey(a => a.NotaryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.Signatures)
            .WithOne(s => s.Act)
            .HasForeignKey(s => s.ActId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.LogEntries)
            .WithOne(l => l.Act)
            .HasForeignKey(l => l.ActId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.ComplianceReviews)
            .WithOne(c => c.Act)
            .HasForeignKey(c => c.ActId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.RequestId);
        builder.HasIndex(a => a.NotaryId);
        builder.HasIndex(a => a.JurisdictionId);
        builder.HasIndex(a => a.Status);

        builder.Navigation(a => a.Signatures).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(a => a.LogEntries).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(a => a.ComplianceReviews).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
