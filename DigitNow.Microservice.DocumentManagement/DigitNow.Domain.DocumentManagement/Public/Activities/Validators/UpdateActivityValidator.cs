using DigitNow.Domain.DocumentManagement.Public.Activities.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Validators
{
    public class UpdateActivityValidator : AbstractValidator<UpdateActivityRequest>
    {
        public UpdateActivityValidator()
        {
            RuleFor(item => item.Id).NotNull().NotEmpty();
            RuleFor(item => item.ModificationMotive).NotNull().NotEmpty();
        }
    }
}
