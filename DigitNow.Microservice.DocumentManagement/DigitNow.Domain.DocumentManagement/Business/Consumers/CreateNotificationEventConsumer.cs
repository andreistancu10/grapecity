using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.CreateNotification;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using Domain.Localization.Client;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.Authentication.Contracts.Users.GetUserExtensionById;

namespace DigitNow.Domain.DocumentManagement.Business.Consumers
{
    public sealed class CreateNotificationEventConsumer : IConsumer<ICreateNotificationEvent>
    {
        private readonly ILogger _logger;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly DocumentManagementDbContext _dbContext;
        private readonly ILocalizationManager _localizationManager;
        private readonly INotificationService _notificationService;

        public CreateNotificationEventConsumer(
            ILogger<CreateNotificationEventConsumer> logger,
            DocumentManagementDbContext dbContext,
            IAuthenticationClient authenticationClient,
            ILocalizationManager localizationManager,
            INotificationService notificationService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _authenticationClient = authenticationClient;
            _localizationManager = localizationManager;
            _notificationService = notificationService;
        }

        public async Task Consume(ConsumeContext<ICreateNotificationEvent> context)
        {
            _logger.LogInformation("{ICreateNotificationEvent} event Received", nameof(ICreateNotificationEvent));

            var createNotificationEvent = context.Message;
            var cancellationToken = context.CancellationToken;

            var notificationUser = await _authenticationClient.GetUserExtensionById(createNotificationEvent.UserId, cancellationToken);

            var responseNotificationType = await _dbContext.NotificationTypes.AsNoTracking()
                .Where(x => x.Id == (long)createNotificationEvent.NotificationTypeId)
                .FirstOrDefaultAsync(cancellationToken);

            IGetUserExtensionByIdResponse notificationFromUser = null;

            if (createNotificationEvent.FromUserId.HasValue)
            {
                notificationFromUser = await _authenticationClient.GetUserExtensionById(createNotificationEvent.FromUserId.Value, cancellationToken);
            }

            if (responseNotificationType != null)
            {
                var messageResponseNotificationTypeTranslationLabel = await _localizationManager.GetTranslations(responseNotificationType.TranslationLabel, cancellationToken);

                var translatedMessage = messageResponseNotificationTypeTranslationLabel.Translations.FirstOrDefault(x => x.LanguageId == 2);
                if (notificationUser != null)
                {
                    // to get tenant or user language
                    var message = translatedMessage != null ? translatedMessage.Translation : responseNotificationType.TranslationLabel;

                    // format params
                    if (translatedMessage != null)
                    {
                        var notificationEventParams = createNotificationEvent.Params.ToList();
                        if (notificationEventParams.Count > 0)
                            message = string.Format(message,
                                notificationEventParams.OrderBy(x => x.Order).Select(x => x.Value).ToArray<object>());
                    }

                    var entity = BuildNotification(message, createNotificationEvent, responseNotificationType, notificationUser, notificationFromUser);

                    _dbContext.Add(entity);

                    await _dbContext.SaveChangesAsync(cancellationToken);
                    await _notificationService.BuildAndSendNotificationToSyncAsync(entity.Id, cancellationToken);
                }
            }
        }

        private static Notification BuildNotification(
            string message,
            ICreateNotificationEvent createNotificationEvent,
            NotificationType responseNotificationType,
            IGetUserExtensionByIdResponse notificationUser,
            IGetUserExtensionByIdResponse notificationFromUser) => new Notification
            {
                Message = message,
                NotificationTypeId = (long)createNotificationEvent.NotificationTypeId,
                NotificationStatusId = responseNotificationType.NotificationStatusId,
                UserId = notificationUser.Id,
                FromUserId = notificationFromUser?.Id,
                EntityId = createNotificationEvent.EntityId,
                EntityTypeId = createNotificationEvent.NotificationTypeId.GetNotificationEntityTypeEnumByNotificationType(),
                Seen = false,
                CreatedOn = DateTime.Now,
                ReactiveSettings = createNotificationEvent.ReactiveSettings != null ? JsonSerializer.Serialize(createNotificationEvent.ReactiveSettings) : string.Empty
            };
    }
}