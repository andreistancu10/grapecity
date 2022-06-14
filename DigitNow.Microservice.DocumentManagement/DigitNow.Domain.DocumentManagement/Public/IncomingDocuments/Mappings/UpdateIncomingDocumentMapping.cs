using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Models;

namespace DigitNow.Domain.DocumentManagement.Public.IncomingDocuments.Mappings
{
    public class UpdateIncomingDocumentMapping : Profile
    {
        public UpdateIncomingDocumentMapping()
        {
            CreateMap<UpdateIncomingDocumentRequest, UpdateIncomingDocumentCommand>();
            CreateMap<UpdateContactDetailsRequest, UpdateContactDetailCommand>();
        }
    }
}