using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings
{
    public class IncomingDocumentMapping : Profile
    {
        public IncomingDocumentMapping()
        {
            // Create
            CreateMap<CreateIncomingDocumentRequest, CreateIncomingDocumentCommand>();

            // Update
            CreateMap<UpdateIncomingDocumentRequest, UpdateIncomingDocumentCommand>();
        }
    }
}
