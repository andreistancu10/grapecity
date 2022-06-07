using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Queries
{
    public class GetDocsByRegistrationNumberMapping : Profile
    {
        public GetDocsByRegistrationNumberMapping()
        {
            CreateMap<IncomingDocument, GetDocsByRegistrationNumberResponse>();
        }
    }
}
