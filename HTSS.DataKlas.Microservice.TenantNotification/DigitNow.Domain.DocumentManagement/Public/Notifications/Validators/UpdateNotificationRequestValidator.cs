using FluentValidation;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Validators
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