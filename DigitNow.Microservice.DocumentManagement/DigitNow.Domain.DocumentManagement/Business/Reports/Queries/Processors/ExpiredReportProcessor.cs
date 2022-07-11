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
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public class ExpiredReportProcessor : IReportProcessor
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;
    private readonly ICatalogClient _catalogClient;
    private readonly DocumentManagementDbContext _dbContext;

    public ExpiredReportProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
        _catalogClient = catalogClient;
    }

    public async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        var filter = new DocumentFilter
        {
            RegistrationDateFilter = request.DateFilter
        };

        var internalCategories = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);
        var categories = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

        var outgoingQuery = _dbContext.Documents.Where(c =>
            c.DocumentType == DocumentType.Outgoing
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                    ? c.IncomingDocument.ResolutionPeriod
                    : categories.DocumentTypes.First(d => d.Id == c.OutgoingDocument.DocumentTypeId).ResolutionPeriod + 1)
                + 1) >=
            request.DateFilter.From
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                    ? c.IncomingDocument.ResolutionPeriod
                    : categories.DocumentTypes.First(d => d.Id == c.OutgoingDocument.DocumentTypeId).ResolutionPeriod + 1)
                + 1) <=
            request.DateFilter.To
            &&
            c.Status != DocumentStatus.Finalized);

        var incomingQuery = _dbContext.Documents.Where(c =>
            c.DocumentType == DocumentType.Outgoing
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                    ? c.IncomingDocument.ResolutionPeriod
                    : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod + 1)
                    + 1) >= request.DateFilter.From
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                   ? c.IncomingDocument.ResolutionPeriod
                   : categories.DocumentTypes.First(d => d.Id == c.IncomingDocument.DocumentTypeId).ResolutionPeriod + 1)
                    + 1) <= request.DateFilter.To
            &&
            c.Status != DocumentStatus.Finalized);

        var internalQuery = _dbContext.Documents.Where(c =>
            c.DocumentType == DocumentType.Outgoing
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                    ? c.IncomingDocument.ResolutionPeriod
                    : internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod + 1)
                + 1) >=
            request.DateFilter.From
            &&
            c.RegistrationDate.AddDays(
                (c.IncomingDocument.ResolutionPeriod > 0
                    ? c.IncomingDocument.ResolutionPeriod
                    : internalCategories.InternalDocumentTypes.First(d => d.Id == c.InternalDocument.InternalDocumentTypeId).ResolutionPeriod + 1)
                + 1) <=
            request.DateFilter.To
            &&
            c.Status != DocumentStatus.Finalized);

        var documents = await outgoingQuery.ToListAsync(cancellationToken);
        documents.AddRange(await incomingQuery.ToListAsync(cancellationToken));
        documents.AddRange(await internalQuery.ToListAsync(cancellationToken));


        //var documents = await _dashboardService.GetAllDocumentsAsync(
        //    request.Page,
        //    request.Count,
        //    cancellationToken,
        //    c => c.Status != DocumentStatus.Finalized);

        return await _documentMappingService.MapToReportViewModelAsync(documents, cancellationToken);
    }
}