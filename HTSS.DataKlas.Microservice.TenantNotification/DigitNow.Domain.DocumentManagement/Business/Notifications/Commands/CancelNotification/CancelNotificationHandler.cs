using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.CancelNotification
{
    internal sealed class CancelNotificationHandler : ICommandHandler<CancelNotificationCommand, ResultObject>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public CancelNotificationHandler(TenantNotificationDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(CancelNotificationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Notification with id {request.Id} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.cancel-notification.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            var currentUser = _identityService.AuthenticatedUser;
            if (entity.UserId != currentUser.UserId)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Notification can be cancelled only by requester!",
                    TranslationCode =
                        "tenant-notification.notification.backend.cancel-notification.validation.not-correct-user",
                    Parameters = new object[] {currentUser.UserId}
                });

            var responseNotificationStatus = await _dbContext.NotificationStatuses
                .AsNoTracking()
                .Where(x => x.Id == (long) NotificationStatusEnum.Cancelled)
                .FirstOrDefaultAsync(cancellationToken);

            if (responseNotificationStatus == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationStatus with id {(long) NotificationStatusEnum.Cancelled} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.cancel-notification.validation.notification-status.notFound",
                    Parameters = new object[] {(long) NotificationStatusEnum.Cancelled}
                });

            if (entity.NotificationStatusId != (long) NotificationStatusEnum.Pending)
                return ResultObject.Error(new ErrorMessage
                {
                    Message =
                        $"Current notification status must be {NotificationStatusEnum.Pending}.",
                    TranslationCode =
                        "tenant-notification.notification.backend.cancel-notification.validation.notification-status.incorrectStatus",
                    Parameters = new object[] {NotificationStatusEnum.Pending}
                });

            entity.NotificationStatusId = responseNotificationStatus.Id;
            entity.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}