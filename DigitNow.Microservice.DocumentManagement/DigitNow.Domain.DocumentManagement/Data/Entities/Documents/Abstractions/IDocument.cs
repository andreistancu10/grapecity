using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Entities
{
    public interface IDocument : IExtendedEntity
    {
        DocumentType DocumentType { get; set; }
        DocumentStatus Status { get; set; }
        int RegistrationNumber { get; set; }
        DateTime RegistrationDate { get; set; }
    }
}
