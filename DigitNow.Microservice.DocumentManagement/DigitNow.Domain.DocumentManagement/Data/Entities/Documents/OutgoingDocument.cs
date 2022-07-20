﻿using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class OutgoingDocument : VirtualDocument
{
    public string? IdentificationNumber { get; set; }
    public long ContactDetailId { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }

    public string RecipientName { get; set; }
    public int RecipientTypeId { get; set; }

    public int DocumentTypeId { get; set; }
    public string DocumentTypeDetail { get; set; }

    #region [ References ]

    public ContactDetail ContactDetail { get; set; }
    public List<ConnectedDocument>? ConnectedDocuments { get; set; } = new();

    #endregion
}