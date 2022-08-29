using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents;
using DigitNow.Domain.DocumentManagement.utils;
using Domain.Localization.Client;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class DocumentsExportHandler : IQueryHandler<DocumentsExportQuery, List<ExportDocumentViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;
        private readonly IDocumentMappingService _documentMappingService;
        private readonly ILocalizationManager _localizationManager;

        public DocumentsExportHandler(
            IMapper mapper,
            IDashboardService dashboardService,
            IDocumentMappingService documentMappingService,
            ILocalizationManager localizationManager)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
            _documentMappingService = documentMappingService;
            _localizationManager = localizationManager;
        }

        public async Task<List<ExportDocumentViewModel>> Handle(DocumentsExportQuery request, CancellationToken cancellationToken)
        {
            var activeDocumentsCount = await _dashboardService.CountActiveDocumentsAsync(DocumentFilter.Empty, cancellationToken);

            var handler = new GetDocumentsHandler(_dashboardService, _documentMappingService);
            var documentsResponse = await handler.Handle(new GetDocumentsQuery
            {
                Count = (int)activeDocumentsCount,
                Filter = request.Filter,
                Page = 1
            }, cancellationToken);

            return _mapper.Map<List<ExportDocumentViewModel>>(documentsResponse.Items);
        }
    }
}