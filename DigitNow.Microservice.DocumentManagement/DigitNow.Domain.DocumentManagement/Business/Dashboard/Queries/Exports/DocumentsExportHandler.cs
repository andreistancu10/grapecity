using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.Exports
{
    public class DocumentsExportHandler : IQueryHandler<DocumentsExportQuery, List<ExportDocumentViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IDashboardService _dashboardService;
        private readonly IDocumentMappingService _documentMappingService;

        public DocumentsExportHandler(
            IMapper mapper,
            IDashboardService dashboardService,
            IDocumentMappingService documentMappingService)
        {
            _mapper = mapper;
            _dashboardService = dashboardService;
            _documentMappingService = documentMappingService;
        }

        public async Task<List<ExportDocumentViewModel>> Handle(DocumentsExportQuery request, CancellationToken cancellationToken)
        {
            var handler = new GetDocumentsHandler(_dashboardService, _documentMappingService);
            var result = await handler.Handle(new GetDocumentsQuery
            {
                Count = request.Count,
                Filter = request.Filter,
                Page = request.Page
            }, cancellationToken);

            return _mapper.Map<List<ExportDocumentViewModel>>(result.Items);
        }
    }
}