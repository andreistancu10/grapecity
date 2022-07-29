using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberMapping : Profile
    {
        public GetDocsByRegistrationNumberMapping()
        {
            CreateMap<Document, GetDocsByRegistrationNumberResponse>()
                .ForMember(c => c.DocumentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(c => c.DocumentType, opt => opt.MapFrom(src => src.DocumentType))
                .ForMember(c => c.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber));
        }
    }
}
