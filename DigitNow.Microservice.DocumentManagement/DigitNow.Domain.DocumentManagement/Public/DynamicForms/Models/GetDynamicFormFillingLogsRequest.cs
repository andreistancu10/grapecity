namespace DigitNow.Domain.DocumentManagement.Public.DynamicForms.Models
{
    public class GetHistoricalArchiveDocumentsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public HistoricalArchiveDocumentsFilterDto Filter { get; set; }
    }

    public class HistoricalArchiveDocumentsFilterDto
    {
        public HistoricalArchiveDocumentsRegistrationDateFilterDto RegistrationDateFilter { get; set; }
        public HistoricalArchiveDocumentsCategoryFilterDto CategoryFilter { get; set; }
    }

    public class HistoricalArchiveDocumentsCategoryFilterDto
    {
        public int CategoryId { get; set; }
    }

    public class HistoricalArchiveDocumentsRegistrationDateFilterDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
