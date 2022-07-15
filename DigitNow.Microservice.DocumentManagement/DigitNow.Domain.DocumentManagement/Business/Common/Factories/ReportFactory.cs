using System.ComponentModel;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories;

public static class ReportFactory
{
    public static IReportProcessor Create(
        ReportType type,
        IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient,
        DocumentManagementDbContext dbContext)
    {
        return type switch
        {
            ReportType.ExpiredDocuments => new ExpiredReportProcessor(dashboardService, documentMappingService, catalogClient, dbContext),
            ReportType.DocumentsToExpire => new ToExpireReportProcessor(dashboardService, documentMappingService, catalogClient, dbContext),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}