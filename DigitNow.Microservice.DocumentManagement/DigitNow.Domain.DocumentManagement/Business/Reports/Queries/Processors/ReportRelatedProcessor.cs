using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    public interface IReportProcessor
    {
        Task<List<ReportViewModel>> GetDataAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken);
    }

    public abstract class ReportRelatedProcessor : IReportProcessor
    {
        private readonly IVirtualDocumentService _virtualDocumentService;
        private readonly IDocumentMappingService _documentMappingService;
        private readonly ICatalogClient _catalogClient;
        private readonly DocumentManagementDbContext _dbContext;

        protected ReportRelatedProcessor(IServiceProvider serviceProvider)
        {
            _virtualDocumentService = serviceProvider.GetService<IVirtualDocumentService>();
            _documentMappingService = serviceProvider.GetService<IDocumentMappingService>();
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        protected virtual void Validate(DateTime toDate, DateTime fromDate)
        {
        }

        public async Task<List<ReportViewModel>> GetDataAsync(DateTime toDate, DateTime fromDate, CancellationToken cancellationToken)
        {
            Validate(toDate, fromDate);
            return await GetDataInternalAsync(toDate, fromDate, cancellationToken);
        }

        protected virtual async Task<List<ReportViewModel>> GetDataInternalAsync(DateTime toDate, DateTime fromDate, CancellationToken cancellationToken)
        {
            var documents = new List<Document>();
            documents.AddRange(await GetEligibleInternalDocumentsAsync(toDate, fromDate, cancellationToken));
            documents.AddRange(await GetEligibleIncomingDocumentsAsync(toDate, fromDate, cancellationToken));

            if (!documents.Any())
            {
                return new List<ReportViewModel>();
            }

            var vDocuments = await _virtualDocumentService.FetchVirtualDocuments(documents, cancellationToken);

            return await _documentMappingService.MapToReportViewModelAsync(vDocuments, cancellationToken);
        }

        private async Task<IEnumerable<Document>> GetEligibleIncomingDocumentsAsync(DateTime toDate, DateTime fromDate, CancellationToken cancellationToken)
        {
            var categories = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

            var incomingDocuments =
                await _dbContext.Documents
                    .AsNoTracking()
                    .Include(c => c.IncomingDocument)
                    .Where(c => c.DocumentType == DocumentType.Incoming && c.Status != DocumentStatus.Finalized)
                    .ToListAsync(cancellationToken);

            var eligibleDocuments = incomingDocuments
                .Where(c =>
                    c.RegistrationDate.AddDays(
                        (c.IncomingDocument.ResolutionPeriod > 0
                            ? c.IncomingDocument.ResolutionPeriod
                            : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod)
                        + 1) > fromDate
                    &&
                    c.RegistrationDate.AddDays(
                        (c.IncomingDocument.ResolutionPeriod > 0
                            ? c.IncomingDocument.ResolutionPeriod
                            : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod)
                        + 1) < toDate);

            return eligibleDocuments;
        }

        private async Task<IEnumerable<Document>> GetEligibleInternalDocumentsAsync(DateTime toDate, DateTime fromDate, CancellationToken cancellationToken)
        {
            var internalCategories = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);

            var internalDocuments = await _dbContext.Documents
                .AsNoTracking()
                .Include(c => c.InternalDocument)
                .Where(c => c.DocumentType == DocumentType.Internal && c.Status != DocumentStatus.Finalized)
                .ToListAsync(cancellationToken);

            var eligibleDocuments = internalDocuments
                .Where(c =>
                    c.RegistrationDate.AddDays(internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod) > fromDate
                    &&
                    c.RegistrationDate.AddDays(internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod) < toDate);

            return eligibleDocuments;
        }
    }
}