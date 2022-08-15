using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels.Export
{
    public class ExportDocumentViewModel
    {
        public long DocumentId { get; set; }
        public long VirtualDocumentId { get; set; }
        public int DocumentType { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationNumber { get; set; }
        public string Issuer { get; set; }
        public string Recipient { get; set; }
        public string DocumentCategory { get; set; }
        public int ResolutionPeriod { get; set; }
        public int Status { get; set; }
        public bool IsDispatched { get; set; }
        public string User { get; set; }
        public string IdentificationNumber { get; set; }
    }
}
