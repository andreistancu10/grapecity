using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocument.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.InternalDocument.Queries.GetByRegistrationNumber;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocument.Commands.Maping;

public class InternalDocumentMappings: Profile
{
    public InternalDocumentMappings()
    {
        CreateMap<CreateInternalDocumentCommand, Data.InternalDocument.InternalDocument>();
        CreateMap<Data.InternalDocument.InternalDocument, GetInternalDocumentByRegistrationNumberResponse>();
    }
}