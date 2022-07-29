using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

public class UpdateOutgoingDocumentRequest
{
    public string User { get; set; }
    public int RecipientTypeId { get; set; }
    public string RecipientName { get; set; }
    public long? IdentificationNumber { get; set; }
    public string ContentSummary { get; set; }
    public int NumberOfPages { get; set; }
    public int RecipientId { get; set; }
    public int DocumentTypeId { get; set; }  //TODO: Rename it to DocumentCategoryId
    public string DocumentTypeDetail { get; set; }
    public UpdateContactDetailsRequest ContactDetail { get; set; }
    public List<int> ConnectedDocumentIds { get; set; }
    public List<long> UploadedFileIds { get; set; }
}