using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.Documents;

public class OutgoingDocument : Document
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