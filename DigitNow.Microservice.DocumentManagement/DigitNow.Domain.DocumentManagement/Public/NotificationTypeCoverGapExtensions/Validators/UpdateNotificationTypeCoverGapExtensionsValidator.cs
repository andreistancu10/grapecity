using DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Models;
using FluentValidation;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Validators
{
    public class UpdateNotificationTypeCoverGapExtensionsValidator : AbstractValidator<UpdateNotificationTypeCoverGapExtensionsRequest>
    {
        public UpdateNotificationTypeCoverGapExtensionsValidator()
        {
            RuleFor(item => item.Active).NotNull();
            RuleFor(item => item.NotificationTypeId).NotNull();
            RuleFor(item => item.NotificationTypeStatus).NotNull();
            RuleFor(item => item.NotificationTypeActionId).NotNull();
            RuleFor(item => item.NotificationTypeActor).NotNull();
        }
    }
}