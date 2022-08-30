using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export
{
    public class ExportDocumentViewModel
    {
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Issuer { get; set; }
        public string Recipient { get; set; }
        public string DocumentType { get; set; }
        public string DocumentCategory { get; set; }
        public int ResolutionPeriod { get; set; }
        public string Status { get; set; }
        public bool IsDispatched { get; set; }
        public string User { get; set; }
    }
}
