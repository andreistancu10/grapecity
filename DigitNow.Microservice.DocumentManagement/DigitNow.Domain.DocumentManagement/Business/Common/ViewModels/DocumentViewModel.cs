using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ViewModels
{
    public class DocumentViewModel
    {
        public long DocumentId { get; set; }
        public long VirtualDocumentId { get; set; }
        public int DocumentType { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationNumber { get; set; }
        public BasicViewModel Issuer { get; set; }
        public BasicViewModel Recipient { get; set; }
        public BasicViewModel DocumentCategory { get; set; }
        public int ResolutionPeriod { get; set; }
        public int Status { get; set; }
        public bool IsDispatched { get; set; }
        public BasicViewModel User { get; set; }
        public string IdentificationNumber { get; set; }
        public bool Editable { get; set; }
    }
}
