using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors;

public class ToExpireReportProcessor : AbstractExpiredReportRelatedProcessor
{
    public ToExpireReportProcessor(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService,
        ICatalogClient catalogClient,
        DocumentManagementDbContext dbContext)
        : base(dashboardService, documentMappingService, catalogClient, dbContext)
    {
    }

    public override async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        if (request.ToDate.ToUniversalTime() <= DateTime.UtcNow)
        {
            var tomorrowDateTime = DateTime.UtcNow.AddDays(1);
            throw new Exception($"Date range cannot be earlier than tomorrow, which is {tomorrowDateTime.Day}/{tomorrowDateTime.Month}/{tomorrowDateTime.Year}.");
        }

        return await base.GetDataAsync(request, cancellationToken);
    }
}