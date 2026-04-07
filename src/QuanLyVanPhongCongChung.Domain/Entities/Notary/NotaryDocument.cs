namespace QuanLyVanPhongCongChung.Domain.Entities.Notary;

using QuanLyVanPhongCongChung.Domain.Common;
using QuanLyVanPhongCongChung.Domain.Enums;

public class NotaryDocument : BaseEntity
{
    public Guid NotaryId { get; private set; }
    public DocumentCategory DocCategory { get; private set; }
    public string FileName { get; private set; } = null!;
    public DateOnly UploadDate { get; private set; }
    public VerifiedStatus VerifiedStatus { get; private set; }
    public int Version { get; private set; }
    public bool IsCurrentVersion { get; private set; }
    public string? FileUrl { get; private set; }

    public Notary Notary { get; private set; } = null!;

    private NotaryDocument() { }

    public static NotaryDocument Create(Guid notaryId, DocumentCategory docCategory,
        string fileName, string? fileUrl, int version = 1)
    {
        return new NotaryDocument
        {
            NotaryId = notaryId,
            DocCategory = docCategory,
            FileName = fileName,
            UploadDate = DateOnly.FromDateTime(DateTime.UtcNow),
            VerifiedStatus = VerifiedStatus.Pending,
            Version = version,
            IsCurrentVersion = true,
            FileUrl = fileUrl
        };
    }
}
