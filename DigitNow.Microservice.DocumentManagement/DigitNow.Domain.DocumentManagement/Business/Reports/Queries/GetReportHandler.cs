using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    public static Report Create(ReportType type, IDashboardService dashboardService, IDocumentMappingService documentMappingService)
    {
        switch (type)
        {
            case ReportType.ExpiredDocuments:
                return new Report(dashboardService, documentMappingService);
            default:
                return null;
        }
    }
}

public interface IReport
{
    Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken);
}

public class Report : IReport
{
    private readonly IDashboardService _dashboardService;
    private readonly IDocumentMappingService _documentMappingService;

    public Report(IDashboardService dashboardService,
        IDocumentMappingService documentMappingService)
    {
        _dashboardService = dashboardService;
        _documentMappingService = documentMappingService;
    }

    public async Task<List<ReportViewModel>> GetDataAsync(GetReportQuery request, CancellationToken cancellationToken)
    {
        var filter = new DocumentPreprocessFilter
        {
            RegistrationDateFilter = request.DateFilter
        };

        var documents = await _dashboardService.GetAllDocumentsAsync(filter, null, request.Page, request.Count, cancellationToken);

        return await _documentMappingService.MapToReportViewModelAsync(documents, cancellationToken);
    }
}