namespace QuanLyVanPhongCongChung.Application.Features.Export.Commands.GenerateSecureLink;

using FluentValidation;

public class GenerateSecureLinkCommandValidator : AbstractValidator<GenerateSecureLinkCommand>
{
    private static readonly string[] AllowedFormats = ["PDF", "CERTIFIED", "JSON", "CSV", "ZIP"];

    public GenerateSecureLinkCommandValidator()
    {
        RuleFor(x => x.DocumentEntityId).NotEmpty();
        RuleFor(x => x.DocumentId).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Format)
            .NotEmpty()
            .Must(x => AllowedFormats.Contains(x.Trim().ToUpperInvariant()))
            .WithMessage("Format must be one of: PDF, CERTIFIED, JSON, CSV, ZIP.");
        RuleFor(x => x.ExpireInMinutes).InclusiveBetween(1, 7 * 24 * 60);
        RuleFor(x => x.Scope).NotEmpty().MaximumLength(50);
    }
}
