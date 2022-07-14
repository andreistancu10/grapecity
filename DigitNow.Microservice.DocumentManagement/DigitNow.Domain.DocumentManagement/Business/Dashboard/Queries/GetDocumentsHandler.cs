using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries
{
    public class GetDocumentsHandler : IQueryHandler<GetDocumentsQuery, GetDocumentsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;

        private int PreviousYear => DateTime.UtcNow.Year - 1;

        public GetDocumentsHandler(IMapper mapper, IDashboardService dashboardService)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
        }

        public Task<GetDocumentsResponse> Handle(GetDocumentsQuery query, CancellationToken cancellationToken)
        {
            if (query.Filter == null)
            {
                return GetSimpleResponseAsync(query, cancellationToken);
            }
            return GetComplexResponseAsync(query, cancellationToken);
        }

        private async Task<GetDocumentsResponse> GetSimpleResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
        {
            var predicates = PredicateFactory.CreatePredicatesList<Document>(x => x.CreatedAt.Year >= PreviousYear);

            var totalItems = await _dashboardService.CountAllDocumentsAsync(predicates, cancellationToken);

            var items = await _dashboardService.GetAllDocumentsAsync(predicates,
                    query.Page,
                    query.Count,
                cancellationToken);

            return BuildDocumentResponse(query, totalItems, items);
        }

        private async Task<GetDocumentsResponse> GetComplexResponseAsync(GetDocumentsQuery query, CancellationToken cancellationToken)
        {
            var predicates = ExpressionFilterBuilderRegistry
                .GetDocumentPredicatesByFilter(query.Filter)
                .Build();

            var totalItems = await _dashboardService.CountAllDocumentsAsync(predicates, cancellationToken);

            var items = await _dashboardService.GetAllDocumentsAsync(
                predicates,
                query.Page,
                query.Count,
                cancellationToken);

            return BuildDocumentResponse(query, totalItems, items);
        }

        private static GetDocumentsResponse BuildDocumentResponse(GetDocumentsQuery query, long totalItems, IList<DocumentViewModel> items)
        {
            var pageCount = totalItems / query.Count;

            if (items.Count % query.Count > 0)
            {
                pageCount += 1;
            }

            return new GetDocumentsResponse
            {
                TotalItems = totalItems,
                PageNumber = query.Page,
                PageSize = query.Count,
                TotalPages = pageCount,
                Items = items
            };
        }
    }
}