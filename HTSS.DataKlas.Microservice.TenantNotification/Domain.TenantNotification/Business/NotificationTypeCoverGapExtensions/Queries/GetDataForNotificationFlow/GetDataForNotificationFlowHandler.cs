﻿using System;
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
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers.Models;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;
using ShiftIn.Domain.TenantNotification.Data;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;
using ShiftIn.Domain.TenantNotification.utils;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.GetDataForNotificationFlow
{
    internal sealed class GetDataForNotificationFlowHandler : IQueryHandler<GetDataForNotificationFlowQuery, GetDataForNotificationFlowResponse>
    {
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IElasticSearchIndexNameService _elasticSearchIndexNameService;

        public GetDataForNotificationFlowHandler(IElasticClient elasticClient,
            IMapper mapper,
            IElasticSearchIndexNameService elasticSearchIndexNameService)
        {
            _elasticClient = elasticClient;
            _mapper = mapper;
            _elasticSearchIndexNameService = elasticSearchIndexNameService;
        }

        public async Task<GetDataForNotificationFlowResponse> Handle(GetDataForNotificationFlowQuery request, CancellationToken cancellationToken)
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

            GetDataForNotificationFlowResponse response = new GetDataForNotificationFlowResponse();
            response.Columns = new List<NotificationTypeCoverGapExtensionGridColumn>();
            response.Rows = new List<NotificationTypeCoverGapExtensionGridRow>();
            response.Columns.Add(BuildNotificationTypeCoverGapExtensionGridColumn("tenant-notification.notification-type-cover-gap-extension.backend.get-data-for-notification-flow-response.statusColumn", 0));
            response.Columns.AddRange(elasticResponse.Documents.GroupBy(x => x.NotificationTypeActor).Select(x => BuildNotificationTypeCoverGapExtensionGridColumn(x.Key, x.Count())).ToList());
            var rows = elasticResponse.Documents.GroupBy(x => x.NotificationTypeStatus).ToDictionary(x => x.Key, x => x.Where(y => y.Active).ToList());
            foreach (var row in rows)
            {
                response.Rows.Add(new Helpers.Models.NotificationTypeCoverGapExtensionGridRow()
                {
                    RowKey = row.Key,
                    Cells = GetCells(row, response.Columns)
                });
            }
            return response;
        }

        private static NotificationTypeCoverGapExtensionGridColumn BuildNotificationTypeCoverGapExtensionGridColumn(string columnKey, long displayOrder) => new NotificationTypeCoverGapExtensionGridColumn()
        {
            ColumnKey = columnKey,
            DisplayOrder = displayOrder
        };

        private List<FilterNotificationTypeCoverGapExtensionResponse> GetCells(KeyValuePair<string, List<NotificationTypeCoverGapExtensionElastic>> row, List<NotificationTypeCoverGapExtensionGridColumn> columns)
        {
            var result = _mapper.Map<List<FilterNotificationTypeCoverGapExtensionResponse>>(row.Value);
            foreach (var col in columns)
            {
                if(col.DisplayOrder > 0 && !result.Any(x=>x.NotificationTypeActor == col.ColumnKey))
                {
                    result.Add(BuildFilterNotificationTypeCoverGapExtensionResponse(col, row));
                }
            }
            return result;
        }

        private static FilterNotificationTypeCoverGapExtensionResponse BuildFilterNotificationTypeCoverGapExtensionResponse(NotificationTypeCoverGapExtensionGridColumn col, KeyValuePair<string, List<NotificationTypeCoverGapExtensionElastic>> row) => new FilterNotificationTypeCoverGapExtensionResponse()
        {
            NotificationTypeActionId = (long)NotificationTypeCoverGapAction.None,
            NotificationTypeActor = col.ColumnKey,
            NotificationTypeStatus = row.Key,
            NotificationTypeMessage = null
        };
    }
}