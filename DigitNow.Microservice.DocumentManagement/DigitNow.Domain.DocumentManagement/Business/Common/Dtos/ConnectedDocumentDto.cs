using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Dtos
{
    public class ConnectedDocumentDto
    {
        public long ChildDocumentId { get; set; }
        public long RegistrationNumber { get; set; }
        public DocumentType DocumentType { get; set; }
    }
}
