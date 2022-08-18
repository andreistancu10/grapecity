using DigitNow.Domain.DocumentManagement.Public.SpecificObjectives.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Validators
{
    public class UpdateSpecificObjectiveRequestValidator : AbstractValidator<UpdateSpecificObjectiveRequest>
    {
        public UpdateSpecificObjectiveRequestValidator()
        {
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.State).NotNull().NotEmpty();
            RuleFor(item => item.Details).NotNull().NotEmpty();
            RuleFor(item => item.SpecificObjectiveFunctionaryIds).NotNull().NotEmpty();
        }
    }
}
