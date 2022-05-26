using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.Extensions;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using Nest;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Helpers;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.Notifications;
using ShiftIn.Domain.TenantNotification.utils;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.Filter
{
    internal sealed class FilterNotificationHandler : IQueryHandler<FilterNotificationQuery, ResultPagedList<FilterNotificationResponse>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public FilterNotificationHandler(IElasticClient elasticClient,
            IMapper mapper,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task<ResultPagedList<FilterNotificationResponse>> Handle(FilterNotificationQuery request, CancellationToken cancellationToken)
        {
            // define filter
            var filterDescriptor = new FilterDescriptor<NotificationElastic>();

            filterDescriptor.Query(p => p.Id, request.Id, () => request.GetFilterMode(p => p.Id));
            filterDescriptor.Query(p => p.Message, request.Message, () => request.GetFilterMode(p => p.Message));
            filterDescriptor.Query(p => p.NotificationTypeId, request.NotificationTypeId, () => request.GetFilterMode(p => p.NotificationTypeId));
            filterDescriptor.Query(p => p.NotificationTypeName, request.NotificationTypeName, () => request.GetFilterMode(p => p.NotificationTypeName));
            filterDescriptor.Query(p => p.NotificationStatusId, request.NotificationStatusId, () => request.GetFilterMode(p => p.NotificationStatusId));

            if (request.NotificationStatusIds?.Count > 0)
                filterDescriptor.Query(p => p.NotificationStatusId, request.NotificationStatusIds, () => request.GetFilterMode(p => p.NotificationStatusIds));

            if (request.NotificationTypeIds?.Count > 0)
                filterDescriptor.Query(p => p.NotificationTypeId, request.NotificationTypeIds, () => request.GetFilterMode(p => p.NotificationTypeIds));

            filterDescriptor.Query(p => p.NotificationStatusName, request.NotificationStatusName, () => request.GetFilterMode(p => p.NotificationStatusName));
            filterDescriptor.Query(p => p.UserId, request.UserId, () => request.GetFilterMode(p => p.UserId));
            filterDescriptor.Query(p => p.UserName, request.UserName, () => request.GetFilterMode(p => p.UserName));
            filterDescriptor.Query(p => p.FromUserId, request.FromUserId, () => request.GetFilterMode(p => p.FromUserId));
            filterDescriptor.Query(p => p.FromUserName, request.FromUserName, () => request.GetFilterMode(p => p.FromUserName));
            filterDescriptor.Query(p => p.EntityId, request.EntityId, () => request.GetFilterMode(p => p.EntityId));
            filterDescriptor.Query(p => p.EntityTypeId, request.EntityTypeId, () => request.GetFilterMode(p => p.EntityTypeId));
            filterDescriptor.Query(p => p.EntityTypeName, request.EntityTypeName, () => request.GetFilterMode(p => p.EntityTypeName));
            filterDescriptor.Query(p => p.CreatedOn, request.CreatedOn, () => request.GetFilterMode(p => p.CreatedOn));
            filterDescriptor.Query(p => p.ModifiedOn, request.ModifiedOn, () => request.GetFilterMode(p => p.ModifiedOn));
            filterDescriptor.Query(p => p.SeenOn, request.SeenOn, () => request.GetFilterMode(p => p.SeenOn));
            filterDescriptor.Query(p => p.Seen, request.Seen, () => request.GetFilterMode(p => p.Seen));
            filterDescriptor.Query(p => p.IsUrgent, request.IsUrgent, () => request.GetFilterMode(p => p.IsUrgent));
            filterDescriptor.Query(p => p.IsInformative, request.IsInformative, () => request.GetFilterMode(p => p.IsInformative));

            // execute search
            var elasticResponse =
                await _elasticClient.SearchAsync<NotificationElastic>(s =>
                {
                    var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.Notification);
                    var searchDescriptor = s.Index(indexName).Query(q => q.Bool(b => b.Filter(filterDescriptor)));
                    return searchDescriptor.Sort(request).Paginate(request);
                }, cancellationToken);
            
            // paginate
            var pagedList = new PagedList<NotificationElastic>(
                elasticResponse.Documents.ToList(),
                (int) elasticResponse.Total,
                request.PageNumber.GetValueOrDefault(),
                request.PageSize.GetValueOrDefault());

            // return response
            return new ResultPagedList<FilterNotificationResponse>(pagedList.GetHeader(),
                _mapper.Map<List<FilterNotificationResponse>>(pagedList.List));

        }
    }
}