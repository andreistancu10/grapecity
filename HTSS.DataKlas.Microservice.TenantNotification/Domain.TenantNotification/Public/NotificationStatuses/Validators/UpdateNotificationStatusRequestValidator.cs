using FluentValidation;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Validators
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