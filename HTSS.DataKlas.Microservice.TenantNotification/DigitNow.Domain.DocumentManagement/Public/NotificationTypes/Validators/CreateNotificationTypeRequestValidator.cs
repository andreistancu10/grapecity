using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Validators
{
    public class CreateNotificationTypeRequestValidator : AbstractValidator<CreateNotificationTypeRequest>
    {
        public CreateNotificationTypeRequestValidator(TenantNotificationDbContext dbContext)
        {
            RuleFor(item => item.Name).NotNull().NotEmpty();

            RuleFor(item => item).Custom((request, context) =>
            {
                var notificationTypesInDb = dbContext.NotificationTypes.ToList();

                if (notificationTypesInDb.Any(x => string.Equals(x.Code, request.Code, StringComparison.OrdinalIgnoreCase)))
                    context.AddFailure(new ValidationFailure(nameof(request.Name),
                        $"A NotificationType with code {request.Code} already exists.")
                    {
                        ErrorCode = "tenant-notification.notification-type.backend.create.validation.codeIsNotUnique"
                    });
            });
        }
    }
}