namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetSecureExportByToken;

using MediatR;
using QuanLyVanPhongCongChung.Application.Features.Export.Common;

public sealed record GetSecureExportByTokenQuery(string Token) : IRequest<SecureExportAccessDto>;
