using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

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