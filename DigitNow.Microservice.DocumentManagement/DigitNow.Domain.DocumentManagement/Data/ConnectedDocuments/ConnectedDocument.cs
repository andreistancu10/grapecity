using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments
{
    public class ConnectedDocument
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public int DocumentType { get; set; }
        public List<IncomingDocument> IncomingDocuments { get; set; }
    }
}
