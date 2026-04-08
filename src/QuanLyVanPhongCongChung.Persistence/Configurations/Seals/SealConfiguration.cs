namespace QuanLyVanPhongCongChung.Persistence.Configurations.Seals;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Seals;

public class SealConfiguration : IEntityTypeConfiguration<Seal>
{
    public void Configure(EntityTypeBuilder<Seal> builder)
    {
        builder.ToTable("Seals");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.CommissionId).HasColumnName("commission_id");
        builder.Property(s => s.Type).HasColumnName("type");
        builder.Property(s => s.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(s => s.Status).HasColumnName("status");
        builder.Property(s => s.ImageUrl).HasColumnName("image_url").HasMaxLength(500);
        builder.Property(s => s.IssuedAt).HasColumnName("issued_at");
        builder.Property(s => s.AllowedActTypes).HasColumnName("allowed_act_types").HasMaxLength(500);
        builder.Property(s => s.NotarialActId).HasColumnName("notarial_act_id");
        builder.Property(s => s.ReplaceSealId).HasColumnName("replace_seal_id");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(s => s.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(s => s.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(s => s.Commission)
            .WithMany()
            .HasForeignKey(s => s.CommissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.ReplaceSeal)
            .WithMany()
            .HasForeignKey(s => s.ReplaceSealId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(s => s.UsageLogs)
            .WithOne(u => u.Seal)
            .HasForeignKey(u => u.SealId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.CommissionId);
        builder.HasIndex(s => s.ReplaceSealId);
        builder.HasIndex(s => s.NotarialActId);
        builder.HasIndex(s => s.Status);

        builder.Navigation(s => s.UsageLogs).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
