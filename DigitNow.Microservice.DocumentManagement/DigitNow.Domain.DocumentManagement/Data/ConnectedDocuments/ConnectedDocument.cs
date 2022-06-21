using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using HTSS.Platform.Core.Domain;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;

public class ConnectedDocument : Entity
{
    public long ChildDocumentId { get; set; }
    public long RegistrationNumber { get; set; }
    public DocumentType DocumentType { get; set; }

    #region [ Relationships ]

    public IncomingDocument IncomingDocument { get; set; }
    public OutgoingDocument OutgoingDocument { get; set; }

    #endregion
}