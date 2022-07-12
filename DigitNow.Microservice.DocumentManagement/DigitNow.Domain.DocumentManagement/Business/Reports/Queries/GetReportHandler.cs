using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportHandler : IQueryHandler<GetReportQuery, List<ReportViewModel>>
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;
    private readonly ICatalogClient _catalogClient;
    private readonly DocumentManagementDbContext _dbContext;

    public GetReportHandler(
        IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient,
        DocumentManagementDbContext dbContext)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
        _catalogClient = catalogClient;
        _dbContext = dbContext;
    }

    public async Task<List<ReportViewModel>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        var report = ReportFactory.Create(request.Type, _dashboardService, _documentMappingService, _catalogClient, _dbContext);
        return await report.GetDataAsync(request, cancellationToken);
    }
}