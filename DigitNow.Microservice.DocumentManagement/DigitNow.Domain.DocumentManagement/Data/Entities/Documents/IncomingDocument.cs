using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Data.Entities;

public class IncomingDocument : VirtualDocument
{
    public int InputChannelId { get; set; }
    public int IssuerTypeId { get; set; }
    public string IssuerName { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public ContactDetail ContactDetail { get; set; }
    public string IdentificationNumber { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool? IsUrgent { get; set; }
    public bool? IsGDPRAgreed { get; set; }

    #region [ References ]

    public Document Document { get; set; }
    public List<ConnectedDocument>? ConnectedDocuments { get; set; } = new();

    #endregion
}