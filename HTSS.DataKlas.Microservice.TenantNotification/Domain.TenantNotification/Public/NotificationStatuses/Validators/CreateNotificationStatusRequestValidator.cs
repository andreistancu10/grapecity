using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Validators
{
    public class CreateNotificationStatusRequestValidator : AbstractValidator<CreateNotificationStatusRequest>
    {
        public CreateNotificationStatusRequestValidator(TenantNotificationDbContext dbContext)
        {
            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.Code).NotNull().NotEmpty();

            RuleFor(item => item).Custom((request, context) =>
            {
                var notificationStatusesInDb =
                    dbContext.NotificationStatuses.Where(x => x.Code == request.Code).ToList();

                if (notificationStatusesInDb.Any(x =>
                        string.Equals(x.Code, request.Code, StringComparison.OrdinalIgnoreCase)))
                    context.AddFailure(new ValidationFailure(nameof(request.Name),
                        $"A notification status with code {request.Code} already exists.")
                    {
                        ErrorCode = "tenant-notification.notification-status.backend.create.validation.codeIsNotUnique"
                    });
            });
        }
    }
}