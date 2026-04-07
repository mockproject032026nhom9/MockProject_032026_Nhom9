namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.ToTable("service_requests");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.OrganizationId).HasColumnName("organization_id");
        builder.Property(s => s.Status).HasColumnName("status");
        builder.Property(s => s.CreatedAt).HasColumnName("created_at");
        builder.Property(s => s.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(s => s.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(s => s.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(s => s.Organization)
            .WithMany(o => o.Requests)
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(s => s.Verification)
            .WithOne(v => v.Request)
            .HasForeignKey<Verification>(v => v.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Delivery)
            .WithOne(d => d.Request)
            .HasForeignKey<Delivery>(d => d.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Payments)
            .WithOne(p => p.Request)
            .HasForeignKey(p => p.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Documents)
            .WithOne(d => d.Request)
            .HasForeignKey(d => d.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => s.OrganizationId);

        builder.Navigation(s => s.Payments).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(s => s.Documents).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
