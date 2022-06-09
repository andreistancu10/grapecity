using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments
{
    public class IncomingConnectedDocument
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public int DocumentType { get; set; }
        public List<IncomingDocument> IncomingDocuments { get; set; }
    }
}
