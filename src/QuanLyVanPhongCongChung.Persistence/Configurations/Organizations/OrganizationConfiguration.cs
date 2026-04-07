namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasColumnName("id");
        builder.Property(o => o.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(o => o.TaxCode).HasColumnName("tax_code").HasMaxLength(50);
        builder.Property(o => o.CreatedAt).HasColumnName("created_at");
        builder.Property(o => o.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(o => o.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(o => o.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasMany(o => o.Requests)
            .WithOne(r => r.Organization)
            .HasForeignKey(r => r.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Requests).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
