using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;
using DigitNow.Domain.DocumentManagement.utils;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.Extensions;
using HTSS.Platform.Infrastructure.ElasticsearchProvider.MultiTenant;
using Nest;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.Filter
{
    internal sealed class FilterNotificationTypeCoverGapExtensionHandler : IQueryHandler<FilterNotificationTypeCoverGapExtensionQuery, ResultPagedList<FilterNotificationTypeCoverGapExtensionResponse>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public FilterNotificationTypeCoverGapExtensionHandler(IElasticClient elasticClient,
            IMapper mapper,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task<ResultPagedList<FilterNotificationTypeCoverGapExtensionResponse>> Handle(FilterNotificationTypeCoverGapExtensionQuery request, CancellationToken cancellationToken)
        {
            // define filter
            var filterDescriptor = new FilterDescriptor<NotificationTypeCoverGapExtensionElastic>();

            // execute search
            var elasticResponse = await _elasticClient.SearchAsync<NotificationTypeCoverGapExtensionElastic>(s =>
                {
                    var indexName = _elasticSearchIndexNameService.GetElasticSearchIndex(IndexConfiguration.NotificationTypeCoverGapExtension);
                    var searchDescriptor = s.Index(indexName).Query(q => q.Bool(b => b.Filter(filterDescriptor)));
                    return searchDescriptor.Sort(request).Paginate(request);
                }, cancellationToken);

            // paginate
            var pagedList = new PagedList<NotificationTypeCoverGapExtensionElastic>(
                elasticResponse.Documents.ToList(),
                (int)elasticResponse.Total,
                request.PageNumber.GetValueOrDefault(),
                request.PageSize.GetValueOrDefault());

            // return response
            return new ResultPagedList<FilterNotificationTypeCoverGapExtensionResponse>(pagedList.GetHeader(),
                _mapper.Map<List<FilterNotificationTypeCoverGapExtensionResponse>>(pagedList.List));
        }
    }
}