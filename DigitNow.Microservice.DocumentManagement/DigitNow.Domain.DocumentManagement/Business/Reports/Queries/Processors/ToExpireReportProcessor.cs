using System;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries.Processors
{
    public class ToExpireReportProcessor : ReportRelatedProcessor
    {
        public ToExpireReportProcessor(IServiceProvider serviceProvider)
            : base(serviceProvider) { }

        protected override void Validate(DateTime toDate, DateTime fromDate)
        {
            if (toDate.ToUniversalTime() <= DateTime.UtcNow)
            {
                var tomorrowDateTime = DateTime.UtcNow.AddDays(1);
                throw new Exception($"Date range cannot be earlier than tomorrow, which is {tomorrowDateTime.Day}/{tomorrowDateTime.Month}/{tomorrowDateTime.Year}.");
            }
        }
    }
}