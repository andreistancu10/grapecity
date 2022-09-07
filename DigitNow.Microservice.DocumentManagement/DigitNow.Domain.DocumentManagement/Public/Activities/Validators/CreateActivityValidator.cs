using DigitNow.Domain.DocumentManagement.Public.Activities.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Validators
{
    public class CreateActivityValidator : AbstractValidator<CreateActivityRequest>
    {
        public CreateActivityValidator()
        {
            RuleFor(item => item.GeneralObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.SpecificObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.DepartmentId).NotNull().NotEmpty();
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.Details).NotNull().NotEmpty();
            RuleFor(item => item.ActivityFunctionaryIds).NotNull().NotEmpty();
        }
    }
}
