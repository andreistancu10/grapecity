﻿using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class OutgoingDocument : VirtualDocument
{
    public long DocumentId { get; set; }

    public long? IdentificationNumber { get; set; }
    public int ContactDetailId { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }

    public int RecipientId { get; set; }
    public string RecipientName { get; set; }
    public int RecipientTypeId { get; set; }

    public int DocumentTypeId { get; set; }
    public string DocumentTypeDetail { get; set; }

    public DateTime CreationDate { get; set; }

    #region [ References ]

    public Document Document { get; set; }
    public ContactDetail ContactDetail { get; set; }
    public List<WorkflowHistory> WorkflowHistory { get; set; } = new();
    public List<ConnectedDocument> ConnectedDocuments { get; set; } = new();

    #endregion
}