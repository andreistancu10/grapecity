using DigitNow.Domain.DocumentManagement.Data.Entities.DeliveryDetails;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents.Abstractions;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class OutgoingDocument : VirtualDocument, IShippable
{
    public string? IdentificationNumber { get; set; }
    public long ContactDetailId { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public string RecipientName { get; set; }
    public int DocumentTypeId { get; set; }
    public string DocumentTypeDetail { get; set; }

    #region [ References ]

    public List<ConnectedDocument>? ConnectedDocuments { get; set; } = new();
    public ContactDetail ContactDetail { get; set; }
    public DeliveryDetail DeliveryDetails { get; set ; }

    #endregion
}