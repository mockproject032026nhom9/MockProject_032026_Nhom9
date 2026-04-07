namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.GenerateSecureLink;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record GenerateSecureLinkCommand(
    Guid DocumentEntityId,
    string DocumentId,
    string Format,
    int ExpireInMinutes = 60,
    string Scope = "Client") : IRequest<SecureExportLinkDto>;
