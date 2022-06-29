using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Mappings
{
    public class GetDocumentsMapping : Profile
    {
        public GetDocumentsMapping()
        {
            CreateMap<GetDocumentsRequest, GetDocumentsQuery>();
        }
    }
}
