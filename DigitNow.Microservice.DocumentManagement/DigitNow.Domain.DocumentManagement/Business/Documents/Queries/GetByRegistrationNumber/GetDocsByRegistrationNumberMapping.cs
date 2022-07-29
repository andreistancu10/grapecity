using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.GetByRegistrationNumber
{
    public class GetDocsByRegistrationNumberMapping : Profile
    {
        public GetDocsByRegistrationNumberMapping()
        {
            CreateMap<Document, GetDocsByRegistrationNumberResponse>();
        }
    }
}
