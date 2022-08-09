namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export
{
    public class ExportReportViewModel
    {
        public long Id { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string SpecialRegister { get; set; }
        public string DocumentType { get; set; }
        public string DocumentCategory { get; set; }
        public string Issuer { get; set; }
        public string Recipient { get; set; }
        public string Functionary { get; set; }
        public DateTime? AllocationDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string CurrentStatus { get; set; }
        public int Expired { get; set; }
    }
}