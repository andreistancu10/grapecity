using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors
{
    internal class ReportRelatedProcessor
    {
        private readonly IVirtualDocumentService _virtualDocumentService;
        private readonly IDocumentMappingService _documentMappingService;
        private readonly ICatalogClient _catalogClient;
        private readonly DocumentManagementDbContext _dbContext;

        public ReportRelatedProcessor(IServiceProvider serviceProvider)
        {
            _virtualDocumentService = serviceProvider.GetService<IVirtualDocumentService>();
            _documentMappingService = serviceProvider.GetService<IDocumentMappingService>();
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        public async Task<List<ReportViewModel>> ProcessDocumentsAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var documents = new List<Document>();

            documents.AddRange(await GetEligibleIncomingDocumentsAsync(fromDate, toDate, cancellationToken));
            documents.AddRange(await GetEligibleInternalDocumentsAsync(fromDate, toDate, cancellationToken));
            
            if (!documents.Any())
            {
                return new List<ReportViewModel>();
            }

            var virtualDocuments = _virtualDocumentService.ConvertDocumentsToVirtualDocuments(documents);

            return await _documentMappingService.MapToReportViewModelAsync(virtualDocuments, cancellationToken);
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