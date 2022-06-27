using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

public record CreateOutgoingDocumentRequest
{
    public string RecipientName { get; init; }
    public long? IdentificationNumber { get; init; }
    public CreateContactDetailsRequest ContactDetail { get; init; }
    public string ContentSummary { get; init; }
    public int NumberOfPages { get; init; }
    public string RecipientIdentifier { get; init; }
    public int DocumentTypeId { get; init; }
    public string? DocumentTypeDetail { get; init; }
    public List<int>? ConnectedDocumentIds { get; init; }
}