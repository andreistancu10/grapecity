using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportQuery : IQuery<List<ReportViewModel>>
{
    public ReportType Type { get; set; }
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public DocumentRegistrationDateFilter DateFilter { get; set; }
}