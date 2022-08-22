using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationFilter
{
    public class GetDocumentByRegistrationFilterMapping : Profile
    {
        public GetDocumentByRegistrationFilterMapping()
        {
            CreateMap<Document, GetDocumentByRegistrationFilterResponse>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
