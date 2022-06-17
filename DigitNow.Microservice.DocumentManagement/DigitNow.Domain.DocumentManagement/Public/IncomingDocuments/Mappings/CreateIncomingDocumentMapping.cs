using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings;

public class CreateIncomingDocumentMapping : Profile
{
    public CreateIncomingDocumentMapping()
    {
        CreateMap<CreateIncomingDocumentRequest, CreateIncomingDocumentCommand>();
        CreateMap<CreateContactDetailsRequest, CreateContactDetailCommand>();
    }
}