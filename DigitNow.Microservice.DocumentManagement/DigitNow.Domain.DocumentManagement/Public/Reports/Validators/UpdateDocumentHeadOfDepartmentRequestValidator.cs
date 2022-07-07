using DigitNow.Domain.DocumentManagement.Public.Reports.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Validators;

public class ReportsValidator : AbstractValidator<GetReportRequest>
{
    public ReportsValidator()
    {
           
    }
}