using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Validators
{
    public class ExpiredReportValidator : AbstractValidator<GetExpiredReportRequest>
    {
        public ExpiredReportValidator()
        {
            RuleFor(c => c.FromDate).NotNull().NotEmpty()
                .LessThan(DateTime.UtcNow)
                .LessThan(c => c.ToDate);

            RuleFor(c => c.ToDate).NotNull().NotEmpty()
                .GreaterThan(c => c.FromDate)
                .LessThan(DateTime.UtcNow);
        }
    }
}