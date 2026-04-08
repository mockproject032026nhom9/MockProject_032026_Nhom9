namespace QuanLyVanPhongCongChung.Persistence.Configurations.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Identity;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(256).IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("password_hash").HasMaxLength(512).IsRequired();
        builder.Property(u => u.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
        builder.Property(u => u.Status).HasColumnName("status");
        builder.Property(u => u.FullName).HasColumnName("full_name").HasMaxLength(200).IsRequired();
        builder.Property(u => u.DateOfBirth).HasColumnName("dob");
        builder.Property(u => u.Address).HasColumnName("address").HasMaxLength(500);
        builder.Property(u => u.RoleId).HasColumnName("id_role");
        builder.Property(u => u.CreatedAt).HasColumnName("created_at");
        builder.Property(u => u.CreatedBy).HasColumnName("created_by").HasMaxLength(100);
        builder.Property(u => u.LastModifiedAt).HasColumnName("last_modified_at");
        builder.Property(u => u.LastModifiedBy).HasColumnName("last_modified_by").HasMaxLength(100);

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Status);
    }
}
