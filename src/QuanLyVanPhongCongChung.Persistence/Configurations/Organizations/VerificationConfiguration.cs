namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("Verifications");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id");
        builder.Property(v => v.RequestId).HasColumnName("request_id");
        builder.Property(v => v.Result).HasColumnName("result").HasMaxLength(1000);
        builder.Property(v => v.Method).HasColumnName("method").HasMaxLength(100);

        builder.HasIndex(v => v.RequestId).IsUnique();
    }
}
