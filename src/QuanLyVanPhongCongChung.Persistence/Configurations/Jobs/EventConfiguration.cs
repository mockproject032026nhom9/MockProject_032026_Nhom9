namespace QuanLyVanPhongCongChung.Persistence.Configurations.Jobs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Jobs;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("event_id");
        builder.Property(e => e.EventName).HasColumnName("event_name").HasMaxLength(200).IsRequired();

        builder.HasMany(e => e.Notifications)
            .WithOne(n => n.Event)
            .HasForeignKey(n => n.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(e => e.Notifications).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
