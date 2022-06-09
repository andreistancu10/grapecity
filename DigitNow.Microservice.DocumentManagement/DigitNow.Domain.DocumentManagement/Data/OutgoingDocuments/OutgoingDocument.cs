using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

public class OutgoingDocument : Entity
{
    public string RegistrationNumber { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public string User { get; set; }
    public int InputChannelId { get; set; }
    public int RecipientTypeId { get; set; }
    public string RecipientName { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public ContactDetail ContactDetail { get; set; }
    public string IdentificationNumber { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool IsUrgent { get; set; }
    public bool IsGDPRAgreed { get; set; }
    public List<OutgoingConnectedDocument> ConnectedDocuments { get; set; } = new();
}