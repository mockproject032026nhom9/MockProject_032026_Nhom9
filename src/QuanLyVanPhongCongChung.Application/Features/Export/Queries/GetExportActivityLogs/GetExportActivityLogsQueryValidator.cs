namespace QuanLyVanPhongCongChung.Application.Features.Export.Queries.GetExportActivityLogs;

using FluentValidation;

public class GetExportActivityLogsQueryValidator : AbstractValidator<GetExportActivityLogsQuery>
{
    public GetExportActivityLogsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x)
            .Must(x => !x.From.HasValue || !x.To.HasValue || x.From <= x.To)
            .WithMessage("From must be less than or equal to To.");
        RuleFor(x => x.DocumentId).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.DocumentId));
        RuleFor(x => x.Format).MaximumLength(30).When(x => !string.IsNullOrWhiteSpace(x.Format));
        RuleFor(x => x.Status).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.Status));
    }
}
