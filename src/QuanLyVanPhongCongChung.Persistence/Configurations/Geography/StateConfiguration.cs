namespace QuanLyVanPhongCongChung.Persistence.Configurations.Geography;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("States");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");
        builder.Property(s => s.StateCode).HasColumnName("state_code").HasMaxLength(10).IsRequired();
        builder.Property(s => s.StateName).HasColumnName("state_name").HasMaxLength(100).IsRequired();

        builder.HasIndex(s => s.StateCode).IsUnique();
    }
}
