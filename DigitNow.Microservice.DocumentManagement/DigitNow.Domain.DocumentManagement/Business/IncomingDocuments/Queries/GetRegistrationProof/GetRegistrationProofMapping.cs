using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofMapping : Profile
    {
        public GetRegistrationProofMapping()
        {
            CreateMap<IncomingDocument, GetRegistrationProofResponse>()
                .ForMember(x => x.RegistrationNumber, opt => opt.MapFrom(source => source.Document.RegistrationNumber))
                .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(source => source.Document.RegistrationDate));
        }
    }
}
