using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.OutgoingDocuments.Mappings;

public class CreateOutgoingDocumentMapping : Profile
{
    public CreateOutgoingDocumentMapping()
    {
        CreateMap<CreateOutgoingDocumentRequest, CreateOutgoingDocumentCommand>();
    }
}