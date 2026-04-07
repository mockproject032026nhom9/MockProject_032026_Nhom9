namespace QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record ExportAccessControlDto(
    Guid DocumentEntityId,
    string DocumentId,
    bool ClientAccessEnabled,
    bool RegulatorAccessEnabled,
    bool ComplianceNotificationTriggered);
