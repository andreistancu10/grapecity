using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

public class CreateOutgoingDocumentRequest
{
    public string User { get; set; }
    public int RecipientTypeId { get; set; }
    public string RecipientName { get; set; }
    public string IdentificationNumber { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsGDPRAgreed { get; set; }
    public CreateContactDetailsRequest ContactDetail { get; set; }
    public List<string> ConnectedDocumentIds { get; set; } = new();
}