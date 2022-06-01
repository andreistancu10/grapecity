using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Validators
{
    public class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
    {
        public UpdateNotificationRequestValidator()
        {
            RuleFor(item => item.NotificationTypeId).NotNull().GreaterThan(0);
            RuleFor(item => item.NotificationStatusId).NotNull().GreaterThan(0);
            RuleFor(item => item.UserId).NotNull().GreaterThan(0);
        }
    }
}