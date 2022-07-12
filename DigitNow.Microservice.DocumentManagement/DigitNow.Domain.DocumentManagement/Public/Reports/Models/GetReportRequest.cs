using System;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Models;

public class GetReportRequest
{
    public ReportType Type { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}