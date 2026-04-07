namespace QuanLyVanPhongCongChung.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyVanPhongCongChung.API.Authorization;
using QuanLyVanPhongCongChung.API.Contracts.Exports;
using QuanLyVanPhongCongChung.Application.Common.Models;
using QuanLyVanPhongCongChung.Application.Features.Export.Commands.GenerateSecureLink;
using QuanLyVanPhongCongChung.Application.Features.Export.Commands.UpdateExportAccessControl;
using QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportAccessControl;
using QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportActivityLogs;
using QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetSecureExportByToken;
using QuanLyVanPhongCongChung.Application.Features.Export.Queries.PrintAuditReport;

public class ExportsController : ApiControllerBase
{
    [HttpPost("secure-links")]
    [Authorize(Policy = ExportAuthorizationPolicies.GenerateSecureLink)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GenerateSecureLink([FromBody] GenerateSecureLinkRequest request)
    {
        var result = await Mediator.Send(new GenerateSecureLinkCommand(
            request.DocumentEntityId,
            request.DocumentId,
            request.Format,
            request.ExpireInMinutes,
            request.Scope));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("secure-links/{token}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AccessSecureLink([FromRoute] string token)
    {
        var result = await Mediator.Send(new GetSecureExportByTokenQuery(token));
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("activity-logs")]
    [Authorize(Policy = ExportAuthorizationPolicies.Read)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityLogs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTimeOffset? from = null,
        [FromQuery] DateTimeOffset? to = null,
        [FromQuery] string? documentId = null,
        [FromQuery] string? format = null,
        [FromQuery] string? status = null)
    {
        var result = await Mediator.Send(new GetExportActivityLogsQuery(
            pageNumber,
            pageSize,
            from,
            to,
            documentId,
            format,
            status));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("access-control/{documentEntityId:guid}")]
    [Authorize(Policy = ExportAuthorizationPolicies.Read)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccessControl([FromRoute] Guid documentEntityId, [FromQuery] string? documentId = null)
    {
        var result = await Mediator.Send(new GetExportAccessControlQuery(documentEntityId, documentId));
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPut("access-control/{documentEntityId:guid}")]
    [Authorize(Policy = ExportAuthorizationPolicies.AccessControlUpdate)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAccessControl(
        [FromRoute] Guid documentEntityId,
        [FromBody] UpdateExportAccessControlRequest request)
    {
        var result = await Mediator.Send(new UpdateExportAccessControlCommand(
            documentEntityId,
            request.DocumentId,
            request.ClientAccessEnabled,
            request.RegulatorAccessEnabled));

        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("audit-report")]
    [Authorize(Policy = ExportAuthorizationPolicies.AuditReportPrint)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> PrintAuditReport(
        [FromQuery] string format = "CSV",
        [FromQuery] DateTimeOffset? from = null,
        [FromQuery] DateTimeOffset? to = null,
        [FromQuery] string? documentId = null)
    {
        var result = await Mediator.Send(new PrintAuditReportQuery(format, from, to, documentId));
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}
