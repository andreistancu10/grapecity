using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberMapping : Profile
    {
        public GetDocsByRegistrationNumberMapping()
        {
            CreateMap<Document, GetDocsByRegistrationNumberResponse>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
