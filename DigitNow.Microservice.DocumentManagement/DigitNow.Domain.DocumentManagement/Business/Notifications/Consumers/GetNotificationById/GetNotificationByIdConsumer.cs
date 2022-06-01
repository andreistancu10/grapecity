using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Notifications;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using HTSS.Platform.Infrastructure.MassTransit;
using MassTransit;
using Nest;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.GetNotificationById
{
    public class GetNotificationByIdConsumer : IConsumer<IGetNotificationByIdRequest>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public GetNotificationByIdConsumer(IElasticClient elasticClient,
            IMapper mapper,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task Consume(ConsumeContext<IGetNotificationByIdRequest> context)
        {
            var mqResponse = SkipExecution(context.Message)
                ? new RpcResponse<IGetNotificationByIdResponse>()
                : new RpcResponse<IGetNotificationByIdResponse>
                {
                    Body = await GetNotification(context.Message.Id, context.CancellationToken)
                };

            await context.RespondAsync(mqResponse);
        }

        private bool SkipExecution(IGetNotificationByIdRequest message)
        {
            return message is null || message.Id.Equals(0);
        }

        private async Task<GetNotificationByIdConsumerResponse> GetNotification(long notificationId, CancellationToken cancellationToken)
        {
            GetNotificationByIdConsumerResponse result = null;
            var response = await _elasticClient.SearchAsync<NotificationElastic>(s =>
            {
                var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.Notification);
                return s.Index(indexName).Query(t => t.Term(w => w.Id, notificationId));
            }, cancellationToken);

            if (response != null && response.Documents.Count > 0)
                result = _mapper.Map<GetNotificationByIdConsumerResponse>(response.Documents.First());

            return result;
        }
    }
}