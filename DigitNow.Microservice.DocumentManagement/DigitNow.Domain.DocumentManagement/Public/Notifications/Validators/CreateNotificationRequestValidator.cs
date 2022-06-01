using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Validators
{
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidator()
        {
            RuleFor(item => item.NotificationTypeId).NotNull().GreaterThan(0);
            RuleFor(item => item.UserId).NotNull().GreaterThan(0);
        }
    }
}