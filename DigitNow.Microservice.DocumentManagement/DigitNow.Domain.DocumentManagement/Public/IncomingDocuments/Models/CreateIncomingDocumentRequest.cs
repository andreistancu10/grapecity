using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

public class CreateIncomingDocumentRequest
{
    public int InputChannelId { get; set; }
    public int IssuerTypeId { get; set; }
    public string IssuerName { get; set; }
    public string IdentificationNumber { get; set; }
    public int? ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }    
    public int DocumentTypeId { get; set; } //TODO: Rename it to DocumentCategoryId
    public string? Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool? IsUrgent { get; set; }
    public bool? IsGDPRAgreed { get; set; }
    public CreateContactDetailsRequest ContactDetail { get; set; }
    public List<int> ConnectedDocumentIds { get; set; }
}