namespace QuanLyVanPhongCongChung.Persistence.Configurations.Geography;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Geography;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasColumnName("id");
        builder.Property(l => l.LangCode).HasColumnName("lang_code").HasMaxLength(10).IsRequired();
        builder.Property(l => l.LangName).HasColumnName("lang_name").HasMaxLength(100).IsRequired();

        builder.HasIndex(l => l.LangCode).IsUnique();
    }
}
