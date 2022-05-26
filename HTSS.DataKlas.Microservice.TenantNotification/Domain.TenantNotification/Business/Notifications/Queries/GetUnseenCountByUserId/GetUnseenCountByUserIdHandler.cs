using System.Threading;
using System.Threading.Tasks;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using HTSS.Platform.Infrastructure.Security;
using Nest;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.GetUnseenCountByUserId
{
    internal sealed class GetUnseenCountByUserIdHandler : IQueryHandler<GetUnseenCountByUserIdQuery, GetUnseenCountByUserIdResponse>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IIdentityService _identityService;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public GetUnseenCountByUserIdHandler(IElasticClient elasticClient,
            IIdentityService identityService,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _identityService = identityService;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task<GetUnseenCountByUserIdResponse> Handle(GetUnseenCountByUserIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _elasticClient.SearchAsync<NotificationElastic>(s =>
            {
                var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.Notification);
                return s.Index(indexName).Query(t => t.Term(w => w.UserId, _identityService.AuthenticatedUser.UserId) && t.Term(w => w.Seen, false));
            }, cancellationToken);

            return new GetUnseenCountByUserIdResponse {Count = response.Total};
        }
    }
}