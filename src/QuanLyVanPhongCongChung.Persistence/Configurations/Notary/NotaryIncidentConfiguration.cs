namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryIncidentConfiguration : IEntityTypeConfiguration<NotaryIncident>
{
    public void Configure(EntityTypeBuilder<NotaryIncident> builder)
    {
        builder.ToTable("NotaryIncidents");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        builder.Property(i => i.NotaryId).HasColumnName("notary_id");
        builder.Property(i => i.IncidentType).HasColumnName("incident_type").HasMaxLength(100).IsRequired();
        builder.Property(i => i.Description).HasColumnName("description").HasMaxLength(2000).IsRequired();
        builder.Property(i => i.Severity).HasColumnName("severity");
        builder.Property(i => i.Status).HasColumnName("status");
        builder.Property(i => i.ResolvedAt).HasColumnName("resolved_at");

        builder.HasIndex(i => new { i.NotaryId, i.Status });
    }
}
