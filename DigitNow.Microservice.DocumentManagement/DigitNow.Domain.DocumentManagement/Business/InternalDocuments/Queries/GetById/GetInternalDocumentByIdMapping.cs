using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Queries.GetById
{
    public class GetInternalDocumentByIdMapping : Profile
    {
        public GetInternalDocumentByIdMapping()
        {
            CreateMap<InternalDocument, GetInternalDocumentByIdResponse>()
                .ForMember(c => c.WorkflowHistory, opt => opt.MapFrom(src => src.WorkflowHistory))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.Document.RegistrationNumber));
        }
    }
}