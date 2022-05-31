using DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Validators
{
    public class UpdateNotificationStatusRequestValidator : AbstractValidator<UpdateNotificationStatusRequest>
    {
        public UpdateNotificationStatusRequestValidator()
        {
            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.Code).NotNull().NotEmpty();
        }
    }
}