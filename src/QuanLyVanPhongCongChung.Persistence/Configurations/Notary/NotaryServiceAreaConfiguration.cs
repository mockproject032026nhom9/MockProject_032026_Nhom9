namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryServiceAreaConfiguration : IEntityTypeConfiguration<NotaryServiceArea>
{
    public void Configure(EntityTypeBuilder<NotaryServiceArea> builder)
    {
        builder.ToTable("notary_service_areas");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.StateId).HasColumnName("state_id");
        builder.Property(s => s.CountyName).HasColumnName("county_name").HasMaxLength(200);
        builder.Property(s => s.NotaryId).HasColumnName("notary_id");

        builder.HasOne(s => s.State)
            .WithMany()
            .HasForeignKey(s => s.StateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.NotaryId, s.StateId });
    }
}
