using System;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors
{
    public class ExpiredReportProcessor : ReportRelatedProcessor
    {
        public ExpiredReportProcessor(IServiceProvider serviceProvider)
            : base(serviceProvider) { }
    }
}