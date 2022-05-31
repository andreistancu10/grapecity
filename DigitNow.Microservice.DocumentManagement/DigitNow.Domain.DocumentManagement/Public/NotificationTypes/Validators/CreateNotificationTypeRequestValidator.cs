using System;
using System.Linq;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Models;
using FluentValidation;
using FluentValidation.Results;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Validators
{
    public class CreateNotificationTypeRequestValidator : AbstractValidator<CreateNotificationTypeRequest>
    {
        public CreateNotificationTypeRequestValidator(DocumentManagementDbContext dbContext)
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