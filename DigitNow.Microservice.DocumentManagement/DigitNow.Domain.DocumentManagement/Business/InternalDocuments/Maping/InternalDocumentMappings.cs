using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetByRegistrationNumber;
using DigitNow.Domain.DocumentManagement.Data.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Maping;

public class InternalDocumentMappings: Profile
{
    public InternalDocumentMappings()
    {
        CreateMap<CreateInternalDocumentCommand, InternalDocument>();
        CreateMap<InternalDocument, GetInternalDocumentByRegistrationNumberResponse>();
    }
}