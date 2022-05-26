using FluentValidation;
using HTSS.Platform.Infrastructure.Api.Tools;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Validators
{
    public class UpdateNotificationTypeRequestValidator : AbstractValidator<UpdateNotificationTypeRequest>
    {
        public UpdateNotificationTypeRequestValidator(TenantNotificationDbContext dbContext, RouteParameterAccessor routeParameterAccessor)
        {
            var id = routeParameterAccessor.GetRouteParameter<long>("id");

            RuleFor(item => item.Name).NotNull().NotEmpty();
            RuleFor(item => item.Code).NotNull().NotEmpty();
            RuleFor(item => item.Active).NotNull();

            RuleFor(item => item.Code)
                .MustAsync(async (code, cancellationToken) =>
                    await dbContext.NotificationTypes.AllAsync(x => x.Code != code || x.Id == id, cancellationToken))
                .WithErrorCode("tenant-notification.notification-type.backend.update.validation.codeIsNotUnique");
        }
    }
}