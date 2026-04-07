namespace QuanLyVanPhongCongChung.Persistence.Configurations.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");
        builder.Property(r => r.RoleName).HasColumnName("role_name").HasMaxLength(100).IsRequired();

        builder.HasIndex(r => r.RoleName).IsUnique();

        builder.Navigation(r => r.Users).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
