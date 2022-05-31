using System;
using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.ChangeSeen
{
    internal sealed class ChangeNotificationSeenFlagHandler : ICommandHandler<ChangeNotificationSeenFlagCommand, ResultObject>
    {
        private readonly TenantNotificationDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public ChangeNotificationSeenFlagHandler(TenantNotificationDbContext dbContext,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(ChangeNotificationSeenFlagCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (entity is null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"Notification with id {request.Id} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.change-seen-flag.validation.entityNotFound",
                    Parameters = new object[] {request.Id}
                });

            var currentUser = _identityService.AuthenticatedUser;
            if (entity.UserId != currentUser.UserId)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = "Notification can be marked as seen only by requester!",
                    TranslationCode =
                        "tenant-notification.notification.backend.seen-change.validation.not-correct-user",
                    Parameters = new object[] {currentUser.UserId}
                });

            entity.Seen = request.Seen;
            entity.SeenOn = request.Seen ? DateTime.Now : null;
            entity.ModifiedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Ok(entity.Id);
        }
    }
}