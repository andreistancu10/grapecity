using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocumentResolutionByDocumentIdResponse
{
    public long DocumentId { get; set; }

    public DocumentType DocumentType { get; set; }

    public DocumentResolutionType ResolutionType { get; set; }

    public string Remarks { get; set; }
}