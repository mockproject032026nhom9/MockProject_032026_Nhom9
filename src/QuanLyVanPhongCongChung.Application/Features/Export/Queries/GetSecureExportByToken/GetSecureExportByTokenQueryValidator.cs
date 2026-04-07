namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetSecureExportByToken;

using FluentValidation;

public class GetSecureExportByTokenQueryValidator : AbstractValidator<GetSecureExportByTokenQuery>
{
    public GetSecureExportByTokenQueryValidator()
    {
        RuleFor(x => x.Token).NotEmpty().MaximumLength(300);
    }
}
