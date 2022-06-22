using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public class Document : ExtendedEntity
    {
        public DocumentType DocumentType { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        #region [ References ]

        public IncomingDocument IncomingDocument { get; set; }
        public InternalDocument InternalDocument { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }

        #endregion
    }
}
