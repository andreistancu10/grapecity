using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Documents.Validators
{
    public class CreateDeliveryDetailsRequestValidator : AbstractValidator<CreateDeliveryDetailsRequest>
    {
        public CreateDeliveryDetailsRequestValidator()
        {
            RuleFor(item => item.DeliveryMode).NotNull().NotEmpty();
        }
    }
}
