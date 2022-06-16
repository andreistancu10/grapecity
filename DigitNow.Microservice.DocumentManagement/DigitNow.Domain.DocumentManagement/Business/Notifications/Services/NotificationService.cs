﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Consumers;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Sync;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.Authentication.Contracts.Users.GetUserExtensionById;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Services
{
    public interface INotificationService
    {
        Task BuildAndSendNotificationToSyncAsync(long notificationId, CancellationToken cancellationToken);
    }

    public class NotificationService : INotificationService
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly DocumentManagementDbContext _dbContext;

        public NotificationService(DocumentManagementDbContext dbContext,
            IAuthenticationClient authenticationClient,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _authenticationClient = authenticationClient;
            _serviceProvider = serviceProvider;
        }

        public async Task BuildAndSendNotificationToSyncAsync(long notificationId, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Notifications.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == notificationId, cancellationToken);

            if (entity != null)
            {
                var responseNotificationStatus = await _dbContext.NotificationStatuses.AsNoTracking()
                    .Where(x => x.Id == entity.NotificationStatusId).FirstOrDefaultAsync(cancellationToken);
                var responseNotificationType = await _dbContext.NotificationTypes.AsNoTracking()
                    .Where(x => x.Id == entity.NotificationTypeId).FirstOrDefaultAsync(cancellationToken);
                var notificationUser = await _authenticationClient.GetUserExtensionById(entity.UserId, cancellationToken);
                IGetUserExtensionByIdResponse notificationFromUser = null;

                if (entity.FromUserId.HasValue)
                    notificationFromUser = await _authenticationClient.GetUserExtensionById(entity.FromUserId.Value, cancellationToken);
                
                NotificationElasticReactiveSettings reactiveSettings = null;
                if (!string.IsNullOrEmpty(entity.ReactiveSettings))
                    reactiveSettings =
                        JsonConvert.DeserializeObject<NotificationElasticReactiveSettings>(entity.ReactiveSettings);

                await _serviceProvider.GetRequiredService<IPublishEndpoint>().Publish<INotificationElasticUpdateIndexCommand>(
                    new NotificationElasticUpdateIndexCommand
                    {
                        Id = entity.Id,
                        Message = entity.Message,
                        NotificationTypeId = entity.NotificationTypeId,
                        NotificationTypeName = responseNotificationType.Name,
                        NotificationStatusId = entity.NotificationStatusId,
                        NotificationStatusName = responseNotificationStatus.Name,
                        UserId = entity.UserId,
                        UserName = $"{notificationUser?.FirstName} {notificationUser?.LastName}",
                        FromUserId = entity.FromUserId,
                        FromUserName = notificationFromUser != null ? $"{notificationFromUser?.FirstName} {notificationFromUser?.LastName}" : null,
                        FromUserProfilePhotoUrl = notificationFromUser?.ProfilePhotoUrl,
                        EntityId = entity.EntityId,
                        EntityTypeId = entity.EntityTypeId,
                        EntityTypeName = ((NotificationEntityTypeEnum) responseNotificationType.EntityType).ToString(),
                        IsInformative = responseNotificationType.IsInformative,
                        IsUrgent = responseNotificationType.IsUrgent,
                        Seen = entity.Seen,
                        CreatedOn = entity.CreatedOn,
                        ModifiedOn = entity.ModifiedOn,
                        SeenOn = entity.SeenOn,
                        ReactiveSettings = reactiveSettings,
                        SynchronizationStrategy =
                            INotificationElasticUpdateIndexCommand.SynchronizationStrategyCode.ById
                    }, cancellationToken);
            }
        }
    }
}