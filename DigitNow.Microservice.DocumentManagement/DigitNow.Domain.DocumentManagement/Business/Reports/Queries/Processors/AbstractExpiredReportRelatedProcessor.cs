using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public abstract class AbstractExpiredReportRelatedProcessor : IReportProcessor
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;
    private readonly ICatalogClient _catalogClient;
    private readonly DocumentManagementDbContext _dbContext;

    protected AbstractExpiredReportRelatedProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient,
        DocumentManagementDbContext dbContext)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
        _catalogClient = catalogClient;
        _dbContext = dbContext;
    }

    public virtual async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {

        var internalCategories = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);
        var categories = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

        var incomingDocuments =
            await _dbContext.Documents
                .AsNoTracking()
                .Include(c => c.IncomingDocument)
                .Where(c => c.DocumentType == DocumentType.Incoming && c.Status != DocumentStatus.Finalized)
                .ToListAsync(cancellationToken);

        var eligibleIncomingDocuments = incomingDocuments
            .Where(c =>
                c.RegistrationDate.AddDays(
                    (c.IncomingDocument.ResolutionPeriod > 0
                        ? c.IncomingDocument.ResolutionPeriod
                        : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod)
                    + 1) > request.FromDate
                &&
                c.RegistrationDate.AddDays(
                    (c.IncomingDocument.ResolutionPeriod > 0
                        ? c.IncomingDocument.ResolutionPeriod
                        : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod)
                    + 1) < request.ToDate);

        var internalDocuments = await _dbContext.Documents
            .AsNoTracking()
            .Include(c => c.InternalDocument)
            .Where(c => c.DocumentType == DocumentType.Internal && c.Status != DocumentStatus.Finalized)
            .ToListAsync(cancellationToken);

        var eligibleInternalDocuments = internalDocuments
            .Where(c =>
                c.RegistrationDate.AddDays(internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod) > request.FromDate
                &&
                c.RegistrationDate.AddDays(internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod) < request.ToDate);

        var documents = new List<Document>();
        documents.AddRange(eligibleInternalDocuments);
        documents.AddRange(eligibleIncomingDocuments);

        if (!documents.Any())
        {
            return new List<ReportViewModel>();
        }

        var vDocuments = await _dashboardService.FetchVirtualDocuments(documents, null, cancellationToken);

        return await _documentMappingService.MapToReportViewModelAsync(vDocuments, cancellationToken);
    }
}