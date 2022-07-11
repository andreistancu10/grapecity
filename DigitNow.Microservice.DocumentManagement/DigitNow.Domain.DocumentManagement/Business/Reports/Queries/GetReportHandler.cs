using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
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