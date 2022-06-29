using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofMapping : Profile
    {
        public GetRegistrationProofMapping()
        {
            CreateMap<OutgoingDocument, GetRegistrationProofResponse>();
        }
    }
}
