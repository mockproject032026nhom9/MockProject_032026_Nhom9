namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

using System.Text.Json;

public static class ExportPayloadCodec
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string SerializeScope(ExportScopePayload payload)
        => JsonSerializer.Serialize(payload, JsonOptions);

    public static ExportScopePayload? DeserializeScope(string? json)
        => Deserialize<ExportScopePayload>(json);

    public static string SerializeAccessControl(ExportAccessControlPayload payload)
        => JsonSerializer.Serialize(payload, JsonOptions);

    public static ExportAccessControlPayload? DeserializeAccessControl(string? json)
        => Deserialize<ExportAccessControlPayload>(json);

    public static string SerializeAuditMetadata(ExportAuditMetadataPayload payload)
        => JsonSerializer.Serialize(payload, JsonOptions);

    public static ExportAuditMetadataPayload? DeserializeAuditMetadata(string? json)
        => Deserialize<ExportAuditMetadataPayload>(json);

    public static string ToBase64(string content)
        => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));

    private static T? Deserialize<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch
        {
            return default;
        }
    }
}
