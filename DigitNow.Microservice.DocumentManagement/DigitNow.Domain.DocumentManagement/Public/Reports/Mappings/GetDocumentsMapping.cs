using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Mappings
{
    public class GetReportRequest : Profile
    {
        public GetReportRequest()
        {
            CreateMap<GetReportRequest, GetReportQuery>();
        }
    }
}
