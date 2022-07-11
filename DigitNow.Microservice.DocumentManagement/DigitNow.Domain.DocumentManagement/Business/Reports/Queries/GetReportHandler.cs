using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportHandler : IQueryHandler<GetReportQuery, List<ReportViewModel>>
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public GetReportHandler(IDashboardService dashboardService, IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<List<ReportViewModel>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var report = ReportFactory.Create(request.Type, _dashboardService, _documentMappingService);
        return await report.GetDataAsync(request, cancellationToken);
    }
}

public class ReportFactory
{
    public static IReport Create(ReportType type, IDashboardService dashboardService, IDocumentMappingService documentMappingService)
    {
        return type switch
        {
            ReportType.ExpiredDocuments => new ExpiredReportsProcessor(dashboardService, documentMappingService),
            ReportType.DocumentsToExpire => new ToExpireReportsProcessor(dashboardService, documentMappingService),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}

public interface IReport
{
    Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken);
}

public class ExpiredReportsProcessor : IReport
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public ExpiredReportsProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        var filter = new DocumentFilter
        {
            RegistrationDateFilter = request.DateFilter
        };

        var documents = await _dashboardService.GetAllDocumentsAsync(filter, request.Page, request.Count, cancellationToken);

        return await _documentMappingService.MapToReportViewModelAsync(documents, cancellationToken);
    }
}

public class ToExpireReportsProcessor : IReport
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public ToExpireReportsProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}