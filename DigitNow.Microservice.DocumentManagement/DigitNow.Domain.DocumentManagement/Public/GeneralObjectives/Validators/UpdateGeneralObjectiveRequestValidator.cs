using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Validators
{
    internal class UpdateGeneralObjectiveRequestValidator : AbstractValidator<UpdateGeneralObjectiveRequest>
    {
        public UpdateGeneralObjectiveRequestValidator()
        {
            RuleFor(item => item.ObjectiveId).NotNull().NotEmpty();
            RuleFor(item => item.Title).NotNull().NotEmpty();
            RuleFor(item => item.Details).NotNull().NotEmpty();
        }
    }
}
