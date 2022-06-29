using AutoMapper;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Mappings;

public class OutgoingDocumentMappings : Profile
{
    public OutgoingDocumentMappings()
    {
        CreateMap<CreateOutgoingDocumentCommand, OutgoingDocument>();
        CreateMap<OutgoingDocument, ConnectedDocument>();
        CreateMap<CreateContactDetailCommand, ContactDetail>();
        CreateMap<CreateContactDetailCommand, ContactDetailDto>();
    }
}