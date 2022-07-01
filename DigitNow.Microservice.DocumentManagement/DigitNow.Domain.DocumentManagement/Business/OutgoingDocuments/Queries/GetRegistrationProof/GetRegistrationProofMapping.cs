using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofMapping : Profile
    {
        public GetRegistrationProofMapping()
        {
            CreateMap<OutgoingDocument, GetRegistrationProofResponse>()
                .ForMember(x => x.RegistrationNumber, opt => opt.MapFrom(source => source.Document.RegistrationNumber))
                .ForMember(x => x.RegistrationDate, opt => opt.MapFrom(source => source.Document.RegistrationDate)); 
        }
    }
}
