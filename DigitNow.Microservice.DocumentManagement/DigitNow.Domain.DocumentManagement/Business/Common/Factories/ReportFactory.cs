using System.ComponentModel;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Factories;

public static class ReportFactory
{
    public static IReportProcessor Create(ReportType type, IDashboardService dashboardService, IDocumentMappingService documentMappingService)
    {
        return type switch
        {
            ReportType.ExpiredDocuments => new ExpiredReportProcessor(dashboardService, documentMappingService),
            ReportType.DocumentsToExpire => new ToExpireReportProcessor(dashboardService, documentMappingService),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}