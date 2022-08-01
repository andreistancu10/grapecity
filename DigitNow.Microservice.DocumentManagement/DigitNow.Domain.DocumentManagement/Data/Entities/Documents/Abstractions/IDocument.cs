using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public interface IDocument : IExtendedEntity
    {
        DocumentType DocumentType { get; set; }
        long RegistrationNumber { get; set; }
        DateTime RegistrationDate { get; set; }
        DocumentStatus Status { get; set; }
        DateTime StatusModifiedAt { get; set; }
        long StatusModifiedBy { get; set; }
        long DestinationDepartmentId { get; set; }
    }
}
