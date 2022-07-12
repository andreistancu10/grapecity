using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportQuery : IQuery<List<ReportViewModel>>
{
    public ReportType Type { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}