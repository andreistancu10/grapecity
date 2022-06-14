using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocument.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.InternalDocuments.Mappings;

public class CreateInternalDocumentMapping : Profile
{
    public CreateInternalDocumentMapping()
    {
        CreateMap<CreateInternalDocumentRequest, CreateInternalDocumentCommand>();
    }
}