using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Consumers;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using DigitNow.Domain.DocumentManagement.Data.NotificationStatuses;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using ShiftIn.Domain.Authentication.Client;
using ShiftIn.Domain.Authentication.Contracts.Users.GetUserExtensionByIds;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.UpdateElastic
{
    public class NotificationElasticUpdateIndexCommandConsumer : IConsumer<INotificationElasticUpdateIndexCommand>
    {
        private readonly IAuthenticationClient _authenticationClient;
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<NotificationElasticUpdateIndexCommandConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public NotificationElasticUpdateIndexCommandConsumer(IElasticClient elasticClient,
            IMapper mapper,
            ILogger<NotificationElasticUpdateIndexCommandConsumer> logger,
            IAuthenticationClient authenticationClient,
            DocumentManagementDbContext dbContext,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _logger = logger;
            _authenticationClient = authenticationClient;
            _dbContext = dbContext;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task Consume(ConsumeContext<INotificationElasticUpdateIndexCommand> context)
        {
            var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.Notification);
            var cancellationToken = context.CancellationToken;

            switch (context.Message.SynchronizationStrategy)
            {
                case INotificationElasticUpdateIndexCommand.SynchronizationStrategyCode.ById:
                {
                    var bulkResponse = await _elasticClient.IndexManyAsync(
                        new List<NotificationElastic>
                        {
                            _mapper.Map<NotificationElastic>(context.Message)
                        }, indexName, cancellationToken);

                    if (bulkResponse.ItemsWithErrors.Any())
                        throw new InvalidOperationException(string.Join($", {Environment.NewLine}", bulkResponse.ItemsWithErrors));
                    break;
                }
                case INotificationElasticUpdateIndexCommand.SynchronizationStrategyCode.All:
                {
                    var elasticData = await _elasticClient.CountAsync<NotificationElastic>(c => c.Index(indexName), cancellationToken);

                    if (elasticData.Count > 0)
                    {
                        var deleteResponse = await _elasticClient.DeleteByQueryAsync<NotificationElastic>(
                            del => del.Index(indexName).Query(q => q.QueryString(qs => qs.Query("*"))), cancellationToken);
                    
                        if (deleteResponse.Failures.Any())
                            throw new InvalidOperationException(string.Join($", {Environment.NewLine}", deleteResponse.Failures));

                        _logger.LogInformation("{DebugInformation}", deleteResponse.DebugInformation);
                    }

                    var notifications = await _dbContext.Notifications.AsNoTracking().ToListAsync(cancellationToken);
                    var notificationsToInsert = new List<NotificationElastic>();
                    var responseNotificationTypeList = await _dbContext.NotificationTypes.AsNoTracking().ToListAsync(cancellationToken);
                    var responseNotificationStatusList = await _dbContext.NotificationStatuses.AsNoTracking().ToListAsync(cancellationToken);
                    var notificationUserList = await _authenticationClient.GetUserExtensionsByIds(notifications.Select(p => p.UserId).Distinct().ToList(), cancellationToken);
                    var notificationFromUserList = await _authenticationClient.GetUserExtensionsByIds(notifications.Where(p => p.FromUserId.HasValue).Select(p => p.FromUserId.Value).Distinct().ToList(), cancellationToken);
                
                    foreach (var notification in notifications)
                    {
                        var responseNotificationType = responseNotificationTypeList.FirstOrDefault(x => x.Id == notification.NotificationTypeId);
                        var responseNotificationStatus = responseNotificationStatusList.FirstOrDefault(x => x.Id == notification.NotificationStatusId);
                        var notificationUser = notificationUserList.UserExtensions.FirstOrDefault(p => p.Id == notification.UserId);
                    
                        var notificationFromUser = notificationFromUserList.UserExtensions.FirstOrDefault(p => notification.FromUserId.HasValue && p.Id == notification.FromUserId.Value);

                        if (responseNotificationType == null) continue;
                    
                        NotificationElasticReactiveSettings reactiveSettings = null;
                        if (!string.IsNullOrEmpty(notification.ReactiveSettings))
                            reactiveSettings =
                                JsonConvert.DeserializeObject<NotificationElasticReactiveSettings>(notification
                                    .ReactiveSettings);
                    
                        notificationsToInsert.Add(BuildNotificationElastic(notification, responseNotificationType, responseNotificationStatus, notificationUser, notificationFromUser, reactiveSettings));
                    }

                    var bulkResponse = await _elasticClient.IndexManyAsync(notificationsToInsert, indexName, cancellationToken);

                    if (bulkResponse.ItemsWithErrors.Any())
                        throw new InvalidOperationException(string.Join($", {Environment.NewLine}", bulkResponse.ItemsWithErrors));
                    break;
                }
            }
        }

        private NotificationElastic BuildNotificationElastic(Notification notification, 
            NotificationType responseNotificationType, 
            NotificationStatus responseNotificationStatus, 
            IUserExtensionResponse notificationUser, 
            IUserExtensionResponse notificationFromUser,
            NotificationElasticReactiveSettings reactiveSettings) 
            => new NotificationElastic
        {
            Id = notification.Id,
            Message = notification.Message,
            NotificationTypeId = notification.NotificationTypeId,
            NotificationTypeName = responseNotificationType.Name,
            NotificationStatusId = responseNotificationStatus.Id,
            NotificationStatusName = responseNotificationStatus.Name,
            UserId = notification.UserId,
            UserName = $"{notificationUser?.FirstName} {notificationUser?.LastName}",
            FromUserId = notification.FromUserId,
            FromUserName = notificationFromUser != null
                ? $"{notificationFromUser?.FirstName} {notificationFromUser?.LastName}"
                : null,
            FromUserProfilePhotoUrl = notificationFromUser?.ProfilePhotoUrl,
            EntityId = notification.EntityId,
            EntityTypeId = notification.EntityTypeId,
            EntityTypeName =
                ((NotificationEntityTypeEnum) responseNotificationType.EntityType).ToString(),
            IsInformative = responseNotificationType.IsInformative,
            IsUrgent = responseNotificationType.IsUrgent,
            CreatedOn = notification.CreatedOn,
            SeenOn = notification.SeenOn,
            ModifiedOn = notification.ModifiedOn,
            Seen = notification.Seen,
            ReactiveSettings = reactiveSettings
        };
    }
}