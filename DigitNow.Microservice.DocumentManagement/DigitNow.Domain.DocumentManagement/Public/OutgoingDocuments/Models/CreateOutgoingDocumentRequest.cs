using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Public.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

public record CreateOutgoingDocumentRequest
{
    public string RecipientName { get; init; }
    public string? IdentificationNumber { get; init; }
    public ContactDetailsRequest ContactDetail { get; init; }
    public string ContentSummary { get; init; }
    public int NumberOfPages { get; init; }
    public int DestinationDepartmentId { get; init; }
    public int DocumentTypeId { get; init; }  //TODO: Rename it to DocumentCategoryId
    public string? DocumentTypeDetail { get; init; }
    public List<int>? ConnectedDocumentIds { get; init; }
    public List<long>? UploadedFileIds { get; set; }
}