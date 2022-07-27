using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetById
{
    public class GetIncomingDocumentByIdMapping : Profile
    {
        public GetIncomingDocumentByIdMapping()
        {
            CreateMap<IncomingDocument, GetIncomingDocumentByIdResponse>()
                .ForMember(c => c.WorkflowHistory, opt => opt.MapFrom(src => src.WorkflowHistory))
                .ForMember(c => c.ConnectedDocuments, opt => opt.MapFrom(src => src.ConnectedDocuments))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber));
        }
    }
}