using System;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors
{
    public class ExpiredReportProcessor : ReportRelatedProcessor
    {
        public ExpiredReportProcessor(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override void Validate(DateTime fromDate, DateTime toDate)
        {
            if (toDate.ToUniversalTime() > DateTime.UtcNow)
            {
                throw new Exception($"Date range cannot be bigger than today, {DateTime.Now.Day}/{DateTime.Now.Month}/{DateTime.Now.Year}.");
            }
        }
    }
}