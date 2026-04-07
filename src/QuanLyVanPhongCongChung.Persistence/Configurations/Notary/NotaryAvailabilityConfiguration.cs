namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryAvailabilityConfiguration : IEntityTypeConfiguration<NotaryAvailability>
{
    public void Configure(EntityTypeBuilder<NotaryAvailability> builder)
    {
        builder.ToTable("notary_availabilities");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");
        builder.Property(a => a.NotaryId).HasColumnName("notary_id");
        builder.Property(a => a.WorkingDaysPerWeek).HasColumnName("working_days_per_week");
        builder.Property(a => a.StartTime).HasColumnName("start_time");
        builder.Property(a => a.EndTime).HasColumnName("end_time");
        builder.Property(a => a.FixedDayOff).HasColumnName("fixed_day_off").HasMaxLength(100);

        builder.HasIndex(a => a.NotaryId).IsUnique();
    }
}
