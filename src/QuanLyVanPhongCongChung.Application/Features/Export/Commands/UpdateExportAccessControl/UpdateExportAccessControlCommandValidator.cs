namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.UpdateExportAccessControl;

using FluentValidation;

public class UpdateExportAccessControlCommandValidator : AbstractValidator<UpdateExportAccessControlCommand>
{
    public UpdateExportAccessControlCommandValidator()
    {
        RuleFor(x => x.DocumentEntityId).NotEmpty();
        RuleFor(x => x.DocumentId).NotEmpty().MaximumLength(100);
    }
}
