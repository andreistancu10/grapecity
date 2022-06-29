using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries.GetRegistrationProof
{
    public class GetRegistrationProofMapping : Profile
    {
        public GetRegistrationProofMapping()
        {
            CreateMap<IncomingDocument, GetRegistrationProofResponse>();
        }
    }
}
