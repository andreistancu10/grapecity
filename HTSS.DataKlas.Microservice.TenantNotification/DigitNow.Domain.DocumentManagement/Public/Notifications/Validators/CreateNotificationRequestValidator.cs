using FluentValidation;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Validators
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