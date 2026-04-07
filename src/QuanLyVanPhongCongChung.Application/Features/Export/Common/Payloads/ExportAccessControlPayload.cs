namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record ExportAccessControlPayload(
    string DocumentId,
    bool ClientAccessEnabled,
    bool RegulatorAccessEnabled,
    bool ComplianceNotificationTriggered);
