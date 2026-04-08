namespace QuanLyVanPhongCongChung.Persistence.Configurations.NotarialActs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.NotarialActs;

public class ActSignatureConfiguration : IEntityTypeConfiguration<ActSignature>
{
    public void Configure(EntityTypeBuilder<ActSignature> builder)
    {
        builder.ToTable("ActSignatures");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.ActId).HasColumnName("act_id");
        builder.Property(s => s.UserId).HasColumnName("user_id");
        builder.Property(s => s.OrderIndex).HasColumnName("order_index");
        builder.Property(s => s.SignatureData).HasColumnName("signature_data").HasMaxLength(4000);
        builder.Property(s => s.Status).HasColumnName("status");

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.ActId);
        builder.HasIndex(s => s.UserId);
    }
}
