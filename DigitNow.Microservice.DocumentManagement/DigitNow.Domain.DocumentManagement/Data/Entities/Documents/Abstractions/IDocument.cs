using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public interface IDocument : IExtendedEntity
    {
        public DocumentType DocumentType { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime StatusModifiedAt { get; set; }
        public long StatusModifiedBy { get; set; }

    }
}
