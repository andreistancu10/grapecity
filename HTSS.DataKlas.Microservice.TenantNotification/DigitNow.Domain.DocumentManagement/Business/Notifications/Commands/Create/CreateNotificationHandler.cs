using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Localization.Client;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using HTSS.Platform.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.Authentication.Contracts.Users.GetUserExtensionById;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Create
{
    internal sealed class CreateNotificationHandler : ICommandHandler<CreateNotificationCommand, ResultObject>
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly TenantNotificationDbContext _dbContext;
        private readonly ILocalizationManager _localizationManager;
        private readonly IIdentityService _identityService;
        
        private const string DefaultLanguageCode = "EN";
        
        public CreateNotificationHandler(TenantNotificationDbContext dbContext,
            IAuthenticationClient authenticationClient,
            ILocalizationManager localizationManager, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _authenticationClient = authenticationClient;
            _localizationManager = localizationManager;
            _identityService = identityService;
        }

        public async Task<ResultObject> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var responseNotificationType = await _dbContext.NotificationTypes
                .AsNoTracking()
                .Where(x => x.Id == request.NotificationTypeId)
                .FirstOrDefaultAsync(cancellationToken);

            var notificationUser = await _authenticationClient.GetUserExtensionById(request.UserId, cancellationToken);
            
            IGetUserExtensionByIdResponse notificationFromUser = null;

            if (request.FromUserId.HasValue)
                notificationFromUser = await _authenticationClient.GetUserExtensionById(request.FromUserId.Value, cancellationToken);

            if (responseNotificationType == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"NotificationTypeId with id {request.NotificationTypeId} does not exist.",
                    TranslationCode =
                        "tenant-notification.notification.backend.create.validation.notification-type.notFound",
                    Parameters = new object[] {request.NotificationTypeId}
                });

            var messageResponseNotificationTypeTranslationLabel = await _localizationManager.GetTranslations(responseNotificationType.TranslationLabel, cancellationToken);

            if (notificationUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"User with id {request.UserId} does not exist.",
                    TranslationCode = "tenant-notification.notification.backend.create.validation.user.notFound",
                    Parameters = new object[] {request.UserId}
                });

            if (request.FromUserId.HasValue && notificationFromUser == null)
                return ResultObject.Error(new ErrorMessage
                {
                    Message = $"From User with id {request.UserId} does not exist.",
                    TranslationCode = "tenant-notification.notification.backend.create.validation.from-user.notFound",
                    Parameters = new object[] {request.UserId}
                });
            
            var defaultLanguage = await _localizationManager.GetLanguageByCode(DefaultLanguageCode, cancellationToken);
            var languageId = _identityService.AuthenticatedUser?.LanguageId ?? defaultLanguage.Id;

            var translatedMessage = messageResponseNotificationTypeTranslationLabel
                .Translations
                .FirstOrDefault(x => x.LanguageId == languageId);

            var entity = new Notification
            {
                Message = translatedMessage != null ? translatedMessage.Translation : responseNotificationType.TranslationLabel,
                NotificationTypeId = request.NotificationTypeId,
                NotificationStatusId = responseNotificationType.NotificationStatusId,
                UserId = notificationUser.Id,
                EntityId = request.EntityId,
                EntityTypeId = ((NotificationTypeEnum) request.NotificationTypeId).GetNotificationEntityTypeEnumByNotificationType(),
                FromUserId = notificationFromUser?.Id,
                Seen = false,
                CreatedOn = DateTime.Now
            };

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(entity.Id);
        }
    }
}