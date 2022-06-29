using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries;

public class GetDocumentResolutionByDocumentIdMappings : Profile
{
    public GetDocumentResolutionByDocumentIdMappings()
    {
        CreateMap<DocumentResolution, GetDocumentResolutionByDocumentIdResponse>();
    }
}