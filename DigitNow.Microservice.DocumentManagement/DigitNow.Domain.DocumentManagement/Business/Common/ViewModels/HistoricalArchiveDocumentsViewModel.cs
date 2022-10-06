namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class HistoricalArchiveDocumentsViewModel
    {
        public long FormId { get; set; }
        public long FormFillingLogId { get; set; }
        public BasicViewModel Category { get; set; }
        public DateTime RegistrationAt { get; set; }
        public BasicViewModel RegistrationBy { get; set; }
        public BasicViewModel Department { get; set; }
    }
}
