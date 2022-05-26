using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using HTSS.Platform.Infrastructure.MassTransit;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Nest;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;
using ShiftIn.Utils.Extensions;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Consumers.GetList
{
    public class GetNotificationTypeCoverGapExtensionsRequestResponseConsumer : IConsumer<IGetNotificationTypeCoverGapExtensionsRequest>
    {
        private static readonly IConfigurationProvider _mappingConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<NotificationTypeCoverGapExtensionElastic, INotificationTypeCoverGapExtensionResponse>().As<NotificationTypeCoverGapExtensionResponse>();
            cfg.CreateProjection<NotificationTypeCoverGapExtensionElastic, NotificationTypeCoverGapExtensionResponse>();
        });

        private readonly IElasticClient _elasticClient;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public GetNotificationTypeCoverGapExtensionsRequestResponseConsumer(IElasticClient elasticClient, IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task Consume(ConsumeContext<IGetNotificationTypeCoverGapExtensionsRequest> context)
        {
            var mqResponse = new RpcResponse<IGetNotificationTypeCoverGapExtensionsResponse>
                {
                    Body = !SkipExecution(context.Message) ? await GetNotificationTypeCoverGapExtensions(context.Message.NotificationTypeStatus, context.CancellationToken) : null
                };
            await context.RespondAsync(mqResponse);
        }

        private static bool SkipExecution(IGetNotificationTypeCoverGapExtensionsRequest message)
        {
            return message is null || message.NotificationTypeStatus.IsNullOrEmpty();
        }

        private async Task<GetNotificationTypeCoverGapExtensionsResponse> GetNotificationTypeCoverGapExtensions(string notificationTypeStatus, CancellationToken cancellationToken)
        {
            GetNotificationTypeCoverGapExtensionsResponse result = new GetNotificationTypeCoverGapExtensionsResponse();

            var filterDescriptor =
                    new List<Func<QueryContainerDescriptor<NotificationTypeCoverGapExtensionElastic>, QueryContainer>>
                    {
                            fq => fq.Bool(b => b.Must(mn => mn.Terms(t => t.Field(f => f.Active).Terms(true)))),
                            fq => fq.Wildcard(t => t.Field(f => f.NotificationTypeStatus.ToLower()).Value("*" + notificationTypeStatus.ToLower() + "*"))
                    };

            var response = await _elasticClient.SearchAsync<NotificationTypeCoverGapExtensionElastic>(x =>
            {
                var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.NotificationTypeCoverGapExtension);
                return x.Index(indexName).From(0).Size(int.MaxValue).Query(q => q.Bool(bq => bq.Filter(filterDescriptor)))
                    .TrackTotalHits();
            }, cancellationToken);
            if (response != null && response.Documents.Count > 0)
                result.NotificationTypeCoverGapExtensions = response.Documents.AsQueryable()
                                                                    .ProjectTo<NotificationTypeCoverGapExtensionResponse>(_mappingConfiguration)
                                                                    .ToList<INotificationTypeCoverGapExtensionResponse>();
            return result;
        }
    }
}