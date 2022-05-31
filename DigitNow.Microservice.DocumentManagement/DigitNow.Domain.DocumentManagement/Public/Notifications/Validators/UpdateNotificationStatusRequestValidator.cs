using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Validators
{
    public class UpdateNotificationStatusRequestValidator : AbstractValidator<UpdateNotificationStatusRequest>
    {
        public UpdateNotificationStatusRequestValidator()
        {
            RuleFor(item => item.StatusId).NotNull().GreaterThan(0);
        }
    }
}