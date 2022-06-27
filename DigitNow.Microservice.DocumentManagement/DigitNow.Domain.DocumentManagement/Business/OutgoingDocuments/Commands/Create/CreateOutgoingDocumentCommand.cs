using HTSS.Platform.Core.CQRS;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;

public class CreateOutgoingDocumentCommand : ICommand<ResultObject>
{
    public string User { get; set; }
    public string RecipientName { get; set; }
    public string? IdentificationNumber { get; set; }
    public CreateContactDetailCommand ContactDetail { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }
    public string? DocumentTypeDetail { get; set; }
    public List<int>? ConnectedDocumentIds { get; set; }
}