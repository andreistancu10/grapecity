using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Validators
{
    public class ToExpireReportValidator: AbstractValidator<GetToExpireReportRequest>
    {
        public ToExpireReportValidator()
        {
            RuleFor(c => c.FromDate).NotNull().NotEmpty()
                .LessThan(c => c.ToDate)
                .GreaterThan(DateTime.UtcNow);

            RuleFor(c => c.ToDate).NotNull().NotEmpty()
                .GreaterThan(c => c.FromDate)
                .GreaterThan(DateTime.UtcNow);
        }
    }
}