using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings
{
    public class IncomingDocumentMapping : Profile
    {
        public IncomingDocumentMapping()
        {
            CreateMap<CreateIncomingDocumentRequest, CreateIncomingDocumentCommand>();
        }
    }
}
