using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

public class UpdateOutgoingDocumentRequest
{
    public int InputChannelId { get; set; }
    public int IssuerTypeId { get; set; }
    public string IssuerName { get; set; }
    public string IdentificationNumber { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool? IsUrgent { get; set; }
    public bool? IsGDPRAgreed { get; set; }
    public UpdateContactDetailsRequest ContactDetail { get; set; }
    public List<int> ConnectedDocumentIds { get; set; }
}