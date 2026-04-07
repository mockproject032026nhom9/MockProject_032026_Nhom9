namespace QuanLyVanPhongCongChung.API.Contracts.Exports;

public sealed record UpdateExportAccessControlRequest(
    string DocumentId,
    bool ClientAccessEnabled,
    bool RegulatorAccessEnabled);
