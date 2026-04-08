namespace QuanLyVanPhongCongChung.Persistence.Configurations.Organizations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Organizations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.RequestId).HasColumnName("request_id");
        builder.Property(d => d.FileUrl).HasColumnName("file_url").HasMaxLength(500);
        builder.Property(d => d.Version).HasColumnName("version");

        builder.HasIndex(d => d.RequestId);
    }
}
