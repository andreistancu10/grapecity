using AutoMapper;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Mappings;

public class IncomingDocumentMappings : Profile
{
    public IncomingDocumentMappings()
    {
        CreateMap<CreateIncomingDocumentCommand, IncomingDocument>();
        CreateMap<CreateContactDetailCommand, ContactDetail>();
        CreateMap<UpdateIncomingDocumentCommand, IncomingDocument>();

        CreateMap<UpdateContactDetailCommand, ContactDetail>();
        CreateMap<IncomingDocument, ConnectedDocument>();
        CreateMap<CreateContactDetailCommand, ContactDetailDto>();
    }
}