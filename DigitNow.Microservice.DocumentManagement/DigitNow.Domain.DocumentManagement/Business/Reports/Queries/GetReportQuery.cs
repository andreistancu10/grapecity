using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries
{
    public class GetReportQuery : IQuery<List<ExportReportViewModel>>
    {
        public int LanguageId { get; set; } = 1;
        public ReportType Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}