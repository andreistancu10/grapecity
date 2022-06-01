using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Helpers;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Consumers
{
    public class NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer : IConsumer<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer(IElasticClient elasticClient,
            IMapper mapper,
            DocumentManagementDbContext dbContext,
            ILogger<NotificationTypeCoverGapExtensionElasticUpdateIndexCommandConsumer> logger,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task Consume(ConsumeContext<INotificationTypeCoverGapExtensionElasticUpdateIndexCommand> context)
        {
            var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.NotificationTypeCoverGapExtension);
            var cancellationToken = context.CancellationToken;
            
            if (context.Message.SynchronizationStrategy == INotificationTypeCoverGapExtensionElasticUpdateIndexCommand.SynchronizationStrategyCode.ById)
            {
                var bulkResponse = await _elasticClient.IndexManyAsync(
                    new List<NotificationTypeCoverGapExtensionElastic>
                    {
                            _mapper.Map<NotificationTypeCoverGapExtensionElastic>(context.Message)
                    }, indexName, cancellationToken);

                if (bulkResponse.ItemsWithErrors.Any())
                    throw new InvalidOperationException(string.Join($", {Environment.NewLine}", bulkResponse.ItemsWithErrors));
            }

            if (context.Message.SynchronizationStrategy == INotificationTypeCoverGapExtensionElasticUpdateIndexCommand.SynchronizationStrategyCode.All)
            {
                var elasticData = await _elasticClient.CountAsync<NotificationTypeCoverGapExtensionElastic>(c => c.Index(indexName), cancellationToken);

                if (elasticData.Count > 0)
                {
                    var deleteResponse = await _elasticClient.DeleteByQueryAsync<NotificationTypeCoverGapExtensionElastic>(
                            del => del.Index(indexName).Query(q => q.QueryString(qs => qs.Query("*"))), cancellationToken);
                    
                    if (deleteResponse.Failures.Any())
                        throw new InvalidOperationException(string.Join($", {Environment.NewLine}", deleteResponse.Failures));
                    
                    _logger.LogInformation("{DebugInformation}", deleteResponse.DebugInformation);
                }

                var notificationTypes = await _dbContext.NotificationTypes.Where(x => x.EntityType == (int)NotificationEntityTypeEnum.CoverGapRequest && x.Active).AsNoTracking().ToListAsync(context.CancellationToken);
                var notificationTypeCoverGapExtensions = await _dbContext.NotificationTypeCoverGapExtensions.AsNoTracking().ToListAsync(context.CancellationToken);
                var notificationTypeCoverGapExtensionsToInsert = notificationTypes.Where(x => !notificationTypeCoverGapExtensions.Select(y => y.NotificationTypeId).Contains(x.Id))
                    .Select(x =>
                    {
                        var notificationTypeEnum = (NotificationTypeEnum)Enum
                                             .GetValues(typeof(NotificationTypeEnum)).ToListDynamic()
                                             .FirstOrDefault(z =>
                                                 z.ToString() == x.Code);
                        return new NotificationTypeCoverGapExtension()
                        {
                            NotificationTypeId = x.Id,
                            Active = notificationTypeEnum.GetDefaultActiveFieldValueForNotificationTypeCoverGapExtension(),
                            CreatedOn = DateTime.Now
                        };
                    }).ToList();
                await _dbContext.BulkInsertAsync(notificationTypeCoverGapExtensionsToInsert, cancellationToken);
                var notificationTypeCoverGapExtensionsToDelete = notificationTypeCoverGapExtensions.Where(x => !notificationTypes.Select(y => y.Id).Contains(x.NotificationTypeId)).ToList();
                await _dbContext.BulkDeleteAsync(notificationTypeCoverGapExtensionsToDelete, cancellationToken);
                notificationTypeCoverGapExtensions = await _dbContext.NotificationTypeCoverGapExtensions.AsNoTracking().Include(x => x.NotificationType).ToListAsync(context.CancellationToken);

                var notificationTypeCoverGapExtensionsElastic = new List<NotificationTypeCoverGapExtensionElastic>();
                foreach (var notificationTypeCoverGapExtension in notificationTypeCoverGapExtensions)
                {
                    var notificationTypeEnum = (NotificationTypeEnum)Enum
                                         .GetValues(typeof(NotificationTypeEnum)).ToListDynamic()
                                         .FirstOrDefault(z =>
                                             z.ToString() == notificationTypeCoverGapExtension.NotificationType.Code);

                    notificationTypeCoverGapExtensionsElastic.Add(BuildNotificationTypeCoverGapExtensionElastic(notificationTypeCoverGapExtension, notificationTypeEnum));
                }

                var bulkResponse = await _elasticClient.IndexManyAsync(notificationTypeCoverGapExtensionsElastic, indexName, cancellationToken);

                if (bulkResponse.ItemsWithErrors.Any())
                    throw new InvalidOperationException(string.Join($", {Environment.NewLine}", bulkResponse.ItemsWithErrors));
            }
        }

        private static NotificationTypeCoverGapExtensionElastic BuildNotificationTypeCoverGapExtensionElastic(
            NotificationTypeCoverGapExtension notificationTypeCoverGapExtension,
            NotificationTypeEnum notificationTypeEnum) => new NotificationTypeCoverGapExtensionElastic()
            {
                Id = notificationTypeCoverGapExtension.Id,
                NotificationTypeId = notificationTypeCoverGapExtension.NotificationTypeId,
                NotificationTypeCode = notificationTypeCoverGapExtension.NotificationType.Code,
                NotificationTypeActor = notificationTypeEnum.GetNotificationTypeCoverGapActor(),
                NotificationTypeActionId = notificationTypeEnum.GetNotificationTypeCoverGapAction(),
                NotificationTypeStatus = notificationTypeEnum.GetNotificationTypeCoverGapStatus(),
                NotificationTypeMessage = notificationTypeCoverGapExtension.NotificationType.TranslationLabel,
                Active = notificationTypeCoverGapExtension.Active
            };
    }
}