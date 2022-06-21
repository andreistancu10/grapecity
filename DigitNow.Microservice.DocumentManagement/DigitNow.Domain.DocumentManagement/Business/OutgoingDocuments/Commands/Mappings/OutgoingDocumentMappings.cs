using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.ConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Mappings;

public class OutgoingDocumentMappings : Profile
{
    public OutgoingDocumentMappings()
    {
        CreateMap<CreateOutgoingDocumentCommand, OutgoingDocument>();
        CreateMap<OutgoingDocument, ConnectedDocument>();
        CreateMap<CreateContactDetailCommand, ContactDetail>();
    }
}