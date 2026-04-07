namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.PrintAuditReport;

using FluentValidation;

public class PrintAuditReportQueryValidator : AbstractValidator<PrintAuditReportQuery>
{
    private static readonly string[] AllowedFormats = ["PDF", "CSV", "JSON"];

    public PrintAuditReportQueryValidator()
    {
        RuleFor(x => x.Format)
            .NotEmpty()
            .Must(x => AllowedFormats.Contains(x.Trim().ToUpperInvariant()))
            .WithMessage("Format must be one of: PDF, CSV, JSON.");

        RuleFor(x => x.DocumentId)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.DocumentId));

        RuleFor(x => x)
            .Must(x => !x.From.HasValue || !x.To.HasValue || x.From <= x.To)
            .WithMessage("From must be less than or equal to To.");
    }
}
