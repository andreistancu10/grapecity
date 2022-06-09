using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Data.ContactDetails;
using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Mappings
{
    public class IncomingDocumentMappings : Profile
    {
        public IncomingDocumentMappings()
        {
            CreateMap<CreateIncomingDocumentCommand, IncomingDocument>();
            CreateMap<CreateContactDetailCommand, ContactDetail>();
            CreateMap<IncomingDocument, IncomingConnectedDocument>();
        }
    }
}
