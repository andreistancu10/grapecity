using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ChangeStatus
{
    internal sealed class ChangeNotificationStatusHandler : ICommandHandler<ChangeNotificationStatusCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public ChangeNotificationStatusHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultObject> Handle(ChangeNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Notification with id {request.Id} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.change-status.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            var responseNotificationStatus = await _dbContext.NotificationStatuses.AsNoTracking()
                .Where(x => x.Id == request.NotificationStatusId).FirstOrDefaultAsync(cancellationToken);

            if (responseNotificationStatus == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationStatus with id {request.NotificationStatusId} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.change-status.validation.notification-status.notFound",
                    Parameters = new object[] {request.NotificationStatusId}
                });

            entity.NotificationStatusId = responseNotificationStatus.Id;
            entity.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}