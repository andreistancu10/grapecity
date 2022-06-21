using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.InternalDocuments;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsMappings : Profile
{
    public GetDocumentsMappings()
    {
        CreateMap<IncomingDocument, GetDocumentResponse>()
            .BeforeMap((s, d) => d.DocumentType = (int)DocumentType.Incoming);

        CreateMap<OutgoingDocument, GetDocumentResponse>()
            .BeforeMap((s, d) => d.DocumentType = (int)DocumentType.Outgoing);

        CreateMap<InternalDocument, GetDocumentResponse>()
            .BeforeMap((s, d) => d.DocumentType = (int)DocumentType.Internal);
    }
}
