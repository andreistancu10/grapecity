using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Validators
{
    public class UpdateDocDepartmentRequestValidator : AbstractValidator<UpdateDocDepartmentRequest>
    {
        public UpdateDocDepartmentRequestValidator()
        {
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty();
            RuleFor(item => item.RegistrationNumbers).NotEmpty();
        }
    }
}
