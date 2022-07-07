using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;

public class UpdateOutgoingDocumentCommand : ICommand<ResultObject>
{
    public int Id { get; set; }    
    public string User { get; set; }
    public int RecipientTypeId { get; set; }
    public string RecipientName { get; set; }
    public long? IdentificationNumber { get; set; }
    public CreateContactDetailCommand ContactDetail { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public List<long> ConnectedDocumentIds { get; set; }
    public List<long> UploadedFileIds { get; set; }
}