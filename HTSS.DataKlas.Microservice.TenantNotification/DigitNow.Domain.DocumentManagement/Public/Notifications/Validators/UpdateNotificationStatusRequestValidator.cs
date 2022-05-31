using FluentValidation;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Validators
{
    public class UpdateNotificationStatusRequestValidator : AbstractValidator<UpdateNotificationStatusRequest>
    {
        public UpdateNotificationStatusRequestValidator()
        {
            RuleFor(item => item.StatusId).NotNull().GreaterThan(0);
        }
    }
}