using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Validators;

public class ReportsValidator : AbstractValidator<GetReportRequest>
{
    public ReportsValidator()
    {
        RuleFor(c => c.Type).NotNull().NotEmpty();
        RuleFor(c => c.FromDate).NotNull().NotEmpty();
        RuleFor(c => c.ToDate).NotNull().NotEmpty().GreaterThan(c=>c.FromDate);
    }
}