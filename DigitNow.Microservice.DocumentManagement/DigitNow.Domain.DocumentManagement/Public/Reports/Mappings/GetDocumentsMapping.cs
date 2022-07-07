using AutoMapper;

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
