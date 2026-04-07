namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class AuthorityScopeConfiguration : IEntityTypeConfiguration<AuthorityScope>
{
    public void Configure(EntityTypeBuilder<AuthorityScope> builder)
    {
        builder.ToTable("authority_scopes");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.CommissionId).HasColumnName("commission_id");
        builder.Property(a => a.AuthorityType).HasColumnName("authority_type");

        builder.HasIndex(a => a.CommissionId);
    }
}
