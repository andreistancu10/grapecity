using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocumentByRegistrationNumberMapping : Profile
    {
        public GetDocumentByRegistrationNumberMapping()
        {
            CreateMap<Document, GetDocumentByRegistrationNumberResponse>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
