using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.RejectNotification
{
    internal sealed class RejectNotificationHandler : ICommandHandler<RejectNotificationCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public RejectNotificationHandler(DocumentManagementDbContext dbContext, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(RejectNotificationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Notification with id {request.Id} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.reject-notification.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            var currentUser = _identityService.AuthenticatedUser;
            if (entity.UserId != currentUser.UserId)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Notification can be rejected only by manager or replacement!",
                    TranslationCode =
                        "tenant-notification.notification.backend.reject-notification.validation.not-correct-user",
                    Parameters = new object[] {currentUser.UserId}
                });

            var responseNotificationStatus = await _dbContext.NotificationStatuses
                .AsNoTracking()
                .Where(x => x.Id == (long) NotificationStatusEnum.Rejected)
                .FirstOrDefaultAsync(cancellationToken);

            if (responseNotificationStatus == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationStatus with id {(long) NotificationStatusEnum.Rejected} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.reject-notification.validation.notification-status.notFound",
                    Parameters = new object[] {(long) NotificationStatusEnum.Rejected}
                });

            if (entity.NotificationStatusId != (long) NotificationStatusEnum.Pending)
                return ResultObject.Error(new ErrorMessage
                {
                    Message =
                        $"Current notification status must be {NotificationStatusEnum.Pending}.",
                    TranslationCode =
                        "tenant-notification.notification.backend.reject-notification.validation.notification-status.incorrectStatus",
                    Parameters = new object[] {NotificationStatusEnum.Approved}
                });

            entity.NotificationStatusId = responseNotificationStatus.Id;
            entity.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}