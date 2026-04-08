namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("Deliveries");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.RequestId).HasColumnName("request_id");
        builder.Property(d => d.Method).HasColumnName("method");
        builder.Property(d => d.Status).HasColumnName("status");

        builder.HasIndex(d => d.RequestId).IsUnique();
    }
}
