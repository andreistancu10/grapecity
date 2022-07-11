using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public class ToExpireReportProcessor : IReportProcessor
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public ToExpireReportProcessor(IDashboardService dashboardService,
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