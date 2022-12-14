using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofMapping : Profile
    {
        public GetRegistrationProofMapping()
        {
            CreateMap<FileContent, GetRegistrationProofResponse>();
        }
    }
}
