using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public class ExpiredReportProcessor : AbstractExpiredReportRelatedProcessor
{
    public ExpiredReportProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient,
        DocumentManagementDbContext dbContext) : base(dashboardService, documentMappingService, catalogClient, dbContext)
    {
    }

    public override async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        if (request.To.ToUniversalTime() > DateTime.UtcNow)
        {
            throw new Exception($"Date range cannot be bigger than today, {DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}.");
        }

        return await base.GetDataAsync(request, cancellationToken);
    }
}