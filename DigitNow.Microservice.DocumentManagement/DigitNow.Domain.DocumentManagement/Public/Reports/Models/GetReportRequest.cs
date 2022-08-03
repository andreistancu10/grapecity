namespace DigitNow.Domain.DocumentManagement.Public.Reports.Models
{
    public class GetExpiredReportRequest
    {
        public int LanguageId { get; set; } = 1;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class GetToExpireReportRequest
    {
        public int LanguageId { get; set; } = 1;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}