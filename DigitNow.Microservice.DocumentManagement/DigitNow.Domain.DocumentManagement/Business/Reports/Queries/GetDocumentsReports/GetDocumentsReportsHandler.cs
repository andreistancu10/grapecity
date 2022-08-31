using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.GetDocumentsReports
{
    public class GetDocumentsReportsHandler : IQueryHandler<GetDocumentsReportsQuery, List<ReportViewModel>>
    {        
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IVirtualDocumentService _virtualDocumentService;
        private readonly IDocumentMappingService _documentMappingService;

        public GetDocumentsReportsHandler(
            DocumentManagementDbContext dbContext,
            IVirtualDocumentService virtualDocumentService,
            IDocumentMappingService documentMappingService)
        {
            _dbContext = dbContext;
            _virtualDocumentService = virtualDocumentService;
            _documentMappingService = documentMappingService;
        }

        public async Task<List<ReportViewModel>> Handle(GetDocumentsReportsQuery request, CancellationToken cancellationToken)
        {
            var documents = new List<Document>();

            documents.AddRange(await GetEligibleIncomingDocumentsAsync(request.FromDate, request.ToDate, cancellationToken));
            documents.AddRange(await GetEligibleInternalDocumentsAsync(request.FromDate, request.ToDate, cancellationToken));

            if (!documents.Any())
            {
                return new List<ReportViewModel>();
            }

            var virtualDocuments = _virtualDocumentService.ConvertDocumentsToVirtualDocuments(documents);

            return await _documentMappingService.MapToReportViewModelAsync(request.LanguageId, virtualDocuments, cancellationToken);
        }

        private async Task<IEnumerable<Document>> GetEligibleIncomingDocumentsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var incomingDocuments =
                await _dbContext.Documents
                    .AsNoTracking()
                    .Include(c => c.IncomingDocument)
                    .Include(c => c.SpecialRegisterMappings)
                        .ThenInclude(x => x.SpecialRegister)
                    .Include(c => c.WorkflowHistories)
                    .Where(c => c.DocumentType == DocumentType.Incoming && c.Status != DocumentStatus.Finalized)
                    .Where(x =>
                        (x.RegistrationDate.AddDays(x.IncomingDocument.ResolutionPeriod) > fromDate)
                        &&
                        (x.RegistrationDate.AddDays(x.IncomingDocument.ResolutionPeriod) < toDate)
                    )
                    .ToListAsync(cancellationToken);

            return incomingDocuments;
        }

        private async Task<IEnumerable<Document>> GetEligibleInternalDocumentsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var internalDocuments = await _dbContext.Documents
                .AsNoTracking()
                .Include(c => c.InternalDocument)
                .Include(c => c.SpecialRegisterMappings)
                        .ThenInclude(x => x.SpecialRegister)
                .Include(c => c.WorkflowHistories)
                .Where(c => c.DocumentType == DocumentType.Internal && c.Status != DocumentStatus.Finalized)
                .Where(x =>
                        (x.RegistrationDate.AddDays(x.InternalDocument.DeadlineDaysNumber) > fromDate)
                        &&
                        (x.RegistrationDate.AddDays(x.InternalDocument.DeadlineDaysNumber) < toDate)
                )
                .ToListAsync(cancellationToken);

            return internalDocuments;
        }
    }
}