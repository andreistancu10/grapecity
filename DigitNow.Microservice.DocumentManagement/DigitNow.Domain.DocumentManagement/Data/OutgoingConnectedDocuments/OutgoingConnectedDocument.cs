using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments
{
    public class OutgoingConnectedDocument
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public int DocumentType { get; set; }
        public List<OutgoingDocument> OutgoingDocuments { get; set; }
    }
}
