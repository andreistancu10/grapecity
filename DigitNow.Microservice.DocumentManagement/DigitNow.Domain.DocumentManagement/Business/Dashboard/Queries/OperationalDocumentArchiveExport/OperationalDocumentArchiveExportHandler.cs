using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries.OperationalDocumentArchiveExport;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using HTSS.Platform.Core.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries.OperationalArchiveExport
{
    public class OperationalDocumentArchiveExportHandler : IQueryHandler<OperationalDocumentArchiveExportQuery, List<ExportDocumentViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;
        private readonly IDocumentMappingService _documentMappingService;

        public OperationalDocumentArchiveExportHandler(
            IMapper mapper,
            IDashboardService dashboardService,
            IDocumentMappingService documentMappingService)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
            _documentMappingService = documentMappingService;
        }

        public async Task<List<ExportDocumentViewModel>> Handle(OperationalDocumentArchiveExportQuery request, CancellationToken cancellationToken)
        {
           
            var archivedDocumentsCount = await _dashboardService.CountArchivedDocumentsAsync(DocumentFilter.Empty, cancellationToken);
            var archivedDocumentsFilteredTotal = await _dashboardService.CountArchivedDocumentsAsync(request.Filter, cancellationToken);

            var documents = await _dashboardService.GetArchivedDocumentsAsync(request.Filter,
                1,
                (int)archivedDocumentsCount,
                cancellationToken);

            var viewModels = await _documentMappingService.MapToDocumentViewModelAsync(request.LanguageId, documents, cancellationToken);

            var documentsResponse = BuildFirstPageDocumentResponse(1, (int)archivedDocumentsCount, archivedDocumentsFilteredTotal, viewModels);

            return _mapper.Map<List<ExportDocumentViewModel>>(documentsResponse.Items);
        }

        private static GetDocumentsResponse BuildFirstPageDocumentResponse(int count, int page, long totalItems, IList<DocumentViewModel> items)
        {
            var pageCount = totalItems / count;

            if (items.Count % count > 0)
            {
                pageCount += 1;
            }

            return new GetDocumentsResponse
            {
                TotalItems = totalItems,
                TotalPages = pageCount,
                PageNumber = page,
                PageSize = count,
                Items = items
            };
        }
    }

}
