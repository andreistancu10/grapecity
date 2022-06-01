using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Utils.Helpers;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Update
{
    internal sealed class UpdateNotificationTypeCoverGapExtensionHandler : ICommandHandler<UpdateNotificationTypeCoverGapExtensionCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public UpdateNotificationTypeCoverGapExtensionHandler(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultObject> Handle(UpdateNotificationTypeCoverGapExtensionCommand request, CancellationToken cancellationToken)
        {
            if (request.NotificationTypeActionId != request.ActionId)
            {
                var actionChangedName = (NotificationTypeCoverGapAction)Enum
                                             .GetValues(typeof(NotificationTypeCoverGapAction)).ToListDynamic()
                                             .FirstOrDefault(z =>
                                                 (int)z == request.ActionId);

                var notificationTypeStatus = (NotificationTypeCoverGapStatus)Enum
                                                 .GetValues(typeof(NotificationTypeCoverGapStatus)).ToListDynamic()
                                                 .FirstOrDefault(z =>
                                                     request.NotificationTypeStatus == EnumDescriptionHelper.Get((NotificationTypeCoverGapStatus)z));
                if (request.ActionId == (int)NotificationTypeCoverGapAction.Reactive)
                {
                    var notificationTypesToCheck = await _dbContext.NotificationTypes.Where(x => x.Active
                                                                                                && x.EntityType == (int)NotificationEntityTypeEnum.CoverGapRequest
                                                                                                && x.Code.Contains(notificationTypeStatus.ToString()))
                                                                                    .Select(x => x.Id)
                                                                                    .ToListAsync(cancellationToken);
                    if (await _dbContext.NotificationTypeCoverGapExtensions.CountAsync(x => x.Active && notificationTypesToCheck.Contains(x.NotificationTypeId), cancellationToken) > 1)
                    {
                        return ResultObject.Error(new ErrorMessage
                        {
                            Message = "Only one reactive actor per status!",
                            TranslationCode =
                                   "tenant-notification.notification-type-cover-gap-extension.backend.update.validation.onlyOneReactiveActorPerStatus",
                        });
                    }
                }

                if (request.NotificationTypeActionId != (int)NotificationTypeCoverGapAction.None)
                {
                    var notificationTypeCoverGapExtensionToInactivate = await _dbContext.NotificationTypeCoverGapExtensions.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken);
                    if (notificationTypeCoverGapExtensionToInactivate is null)
                        return ResultObject.Error(new ErrorMessage
                        {
                            Message = $"NotificationTypeCoverGapExtension with id {request.Id} does not exist.",
                            TranslationCode =
                                "tenant-notification.notification-type-cover-gap-extension.backend.update.validation.entityNotFound",
                            Parameters = new object[] { request.Id }
                        });
                    notificationTypeCoverGapExtensionToInactivate.Active = false;
                    notificationTypeCoverGapExtensionToInactivate.ModifiedOn = DateTime.Now;
                    _dbContext.Update(notificationTypeCoverGapExtensionToInactivate);
                }

                return await UpdateNotificationTypeCoverGapExtension(actionChangedName, notificationTypeStatus, request, cancellationToken);
            }

            return ResultObject.Ok();
        }

        private async Task<ResultObject> UpdateNotificationTypeCoverGapExtension(
            NotificationTypeCoverGapAction actionChangedName,
            NotificationTypeCoverGapStatus notificationTypeStatus,
            UpdateNotificationTypeCoverGapExtensionCommand request,
            CancellationToken cancellationToken)
        {
            if (actionChangedName != NotificationTypeCoverGapAction.None)
            {
                var notificationTypeActor = (NotificationTypeCoverGapActor)Enum
                                             .GetValues(typeof(NotificationTypeCoverGapActor)).ToListDynamic()
                                             .FirstOrDefault(z =>
                                                 request.NotificationTypeActor == EnumDescriptionHelper.Get((NotificationTypeCoverGapActor)z));

                var notificationTypeSearched = await _dbContext.NotificationTypes.FirstOrDefaultAsync(x => x.Active
                                                                                                        && x.EntityType == (int)NotificationEntityTypeEnum.CoverGapRequest
                                                                                                        && x.Code.Contains(actionChangedName.ToString())
                                                                                                        && x.Code.Contains(notificationTypeActor.ToString())
                                                                                                        && x.Code.Contains(notificationTypeStatus.ToString()), cancellationToken);
                if (notificationTypeSearched is null)
                    return ResultObject.Error(new ErrorMessage
                    {
                        Message = $"NotificationType to activate does not exist.",
                        TranslationCode =
                            "tenant-notification.notification-type-cover-gap-extension.backend.update.validation.entityNotFound",
                    });

                var notificationTypeCoverGapExtensionToUpdate = await _dbContext.NotificationTypeCoverGapExtensions.FirstOrDefaultAsync(x => x.NotificationTypeId == notificationTypeSearched.Id, cancellationToken);
                if (notificationTypeCoverGapExtensionToUpdate is null)
                    return ResultObject.Error(new ErrorMessage
                    {
                        Message = $"NotificationTypeCoverGapExtension does not exist.",
                        TranslationCode =
                            "tenant-notification.notification-type-cover-gap-extension.backend.update.validation.entityNotFound",
                    });
                notificationTypeCoverGapExtensionToUpdate.Active = true;
                notificationTypeCoverGapExtensionToUpdate.ModifiedOn = DateTime.Now;
                _dbContext.Update(notificationTypeCoverGapExtensionToUpdate);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return ResultObject.Ok();
        }
    }
}