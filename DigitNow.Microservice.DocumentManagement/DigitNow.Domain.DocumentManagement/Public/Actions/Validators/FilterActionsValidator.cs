using DigitNow.Domain.DocumentManagement.Public.Actions.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Actions.Validators
{
    public class FilterActionsValidator : AbstractValidator<FilterActionsRequest>
    {
        public FilterActionsValidator()
        {
        }
    }
}