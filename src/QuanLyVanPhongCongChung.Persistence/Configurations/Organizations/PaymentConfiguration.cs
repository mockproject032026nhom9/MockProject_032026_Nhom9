namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id");
        builder.Property(p => p.RequestId).HasColumnName("request_id");
        builder.Property(p => p.Amount).HasColumnName("amount").HasPrecision(18, 2);
        builder.Property(p => p.Status).HasColumnName("status");
        builder.Property(p => p.Gateway).HasColumnName("gateway").HasMaxLength(100);
        builder.Property(p => p.Transaction).HasColumnName("transaction").HasMaxLength(200);

        builder.HasIndex(p => p.RequestId);
    }
}
