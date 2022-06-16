using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;

public class UpdateOutgoingDocumentCommand : ICommand<ResultObject>
{
    public int Id { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public int RegistrationNumber { get; set; }
    public string User { get; set; }
    public int RecipientTypeId { get; set; }
    public string RecipientName { get; set; }
    public string IdentificationNumber { get; set; }
    public int ExternalNumber { get; set; }
    public DateTime? ExternalNumberDate { get; set; }
    public CreateContactDetailCommand ContactDetail { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string Detail { get; set; }
    public double ResolutionPeriod { get; set; }
    public bool? IsUrgent { get; set; }
    public bool? IsGDPRAgreed { get; set; }
    public List<int> ConnectedDocumentIds { get; set; }
}