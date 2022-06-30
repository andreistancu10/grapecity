
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocumentResolutionByDocumentIdQuery : IQuery<GetDocumentResolutionByDocumentIdResponse>
{
    public long DocumentId { get; set; }
}