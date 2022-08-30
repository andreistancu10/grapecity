using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.GetDocumentsReports
{
    public class GetDocumentsExcelReportsHandler : IQueryHandler<GetDocumentsExcelReportsQuery, List<ExportReportViewModel>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IVirtualDocumentService _virtualDocumentService;
        private readonly IDocumentMappingService _documentMappingService;
        private readonly IMapper _mapper;
        
        public GetDocumentsExcelReportsHandler(
            DocumentManagementDbContext dbContext,
            IMapper mapper,
            IVirtualDocumentService virtualDocumentService,
            IDocumentMappingService documentMappingService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _virtualDocumentService = virtualDocumentService;
            _documentMappingService = documentMappingService;
        }

        public async Task<List<ExportReportViewModel>> Handle(GetDocumentsExcelReportsQuery request, CancellationToken cancellationToken)
        {
            var handler = new GetDocumentsReportsHandler(_dbContext, _virtualDocumentService, _documentMappingService);
            var reportViewModels = await handler.Handle(new GetDocumentsReportsQuery
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                LanguageId = request.LanguageId,
                Type = request.Type
            }, cancellationToken);

            return _mapper.Map<List<ExportReportViewModel>>(reportViewModels);
        }
    }
}