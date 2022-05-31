using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.Authentication.Contracts.Users.GetUserExtensionById;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Update
{
    internal sealed class UpdateNotificationHandler : ICommandHandler<UpdateNotificationCommand, ResultObject>
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly TenantNotificationDbContext _dbContext;

        public UpdateNotificationHandler(TenantNotificationDbContext dbContext,
            IAuthenticationClient authenticationClient)
        {
            _dbContext = dbContext;
            _authenticationClient = authenticationClient;
        }

        public async Task<ResultObject> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.FindAsync(request.Id);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Notification with id {request.Id} does not exist.",
                    TranslationCode = "tenant-notification.notification.backend.update.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });
            

            IGetUserExtensionByIdResponse notificationFromUser = null;

            if (request.FromUserId.HasValue)
                notificationFromUser = await _authenticationClient.GetUserExtensionById(request.FromUserId.Value, cancellationToken);

            var responseNotificationType = await _dbContext.NotificationTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.NotificationTypeId, cancellationToken);
            
            if (responseNotificationType == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationTypeId with id {request.NotificationTypeId} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.update.validation.notification-type.notFound",
                    Parameters = new object[] {request.NotificationTypeId}
                });
            
            var notificationUser = await _authenticationClient.GetUserExtensionById(request.UserId, cancellationToken);
            
            if (notificationUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"User with id {request.UserId} does not exist.",
                    TranslationCode = "tenant-notification.notification.backend.update.validation.user.notFound",
                    Parameters = new object[] {request.UserId}
                });

            var responseNotificationStatus = await _dbContext.NotificationStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.NotificationStatusId, cancellationToken);
            
            if (responseNotificationStatus == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationStatus with id {request.NotificationStatusId} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.update.validation.notification-status.notFound",
                    Parameters = new object[] {request.NotificationStatusId}
                });

            if (request.FromUserId.HasValue && notificationFromUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"From User with id {request.UserId} does not exist.",
                    TranslationCode = "tenant-notification.notification.backend.update.validation.from-user.notFound",
                    Parameters = new object[] {request.UserId}
                });

            entity.NotificationTypeId = request.NotificationTypeId;
            entity.NotificationStatusId = request.NotificationStatusId;
            entity.UserId = request.UserId;
            entity.FromUserId = request.FromUserId;
            entity.EntityId = request.EntityId;

            if (entity.Seen == false && request.Seen)
            {
                entity.Seen = request.Seen;
                entity.SeenOn = DateTime.Now;
            }

            entity.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}