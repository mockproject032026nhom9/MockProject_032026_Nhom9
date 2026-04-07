namespace QuanLyVanPhongCongChung.Persistence.Configurations.Notary;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuanLyVanPhongCongChung.Domain.Entities.Notary;

public class NotaryDocumentConfiguration : IEntityTypeConfiguration<NotaryDocument>
{
    public void Configure(EntityTypeBuilder<NotaryDocument> builder)
    {
        builder.ToTable("notary_documents");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnName("id");
        builder.Property(d => d.NotaryId).HasColumnName("notary_id");
        builder.Property(d => d.DocCategory).HasColumnName("doc_category");
        builder.Property(d => d.FileName).HasColumnName("file_name").HasMaxLength(300).IsRequired();
        builder.Property(d => d.UploadDate).HasColumnName("upload_date");
        builder.Property(d => d.VerifiedStatus).HasColumnName("verified_status");
        builder.Property(d => d.Version).HasColumnName("version");
        builder.Property(d => d.IsCurrentVersion).HasColumnName("is_current_version");
        builder.Property(d => d.FileUrl).HasColumnName("file_url").HasMaxLength(500);

        builder.HasIndex(d => new { d.NotaryId, d.DocCategory });
    }
}
