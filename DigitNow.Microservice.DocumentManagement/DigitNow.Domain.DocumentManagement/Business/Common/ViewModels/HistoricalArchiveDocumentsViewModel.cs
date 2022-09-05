namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class HistoricalArchiveDocumentsViewModel
    {
        public long Id { get; set; }
        public string Category { get; set; }
        public DateTime RegistrationAt { get; set; }
        public BasicViewModel RegistrationBy { get; set; }        
        public BasicViewModel Recipient { get; set; }
    }
}
