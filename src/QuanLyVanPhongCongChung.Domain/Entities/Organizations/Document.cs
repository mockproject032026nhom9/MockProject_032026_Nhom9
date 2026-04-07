namespace QuanLyVanPhongCongChung.Domain.Entities.Organizations;

using QuanLyVanPhongCongChung.Domain.Common;

public class Document : BaseEntity
{
    public Guid RequestId { get; private set; }
    public string? FileUrl { get; private set; }
    public int Version { get; private set; }

    public ServiceRequest Request { get; private set; } = null!;

    private Document() { }

    public static Document Create(Guid requestId, string? fileUrl, int version = 1)
    {
        return new Document
        {
            RequestId = requestId,
            FileUrl = fileUrl,
            Version = version
        };
    }
}
