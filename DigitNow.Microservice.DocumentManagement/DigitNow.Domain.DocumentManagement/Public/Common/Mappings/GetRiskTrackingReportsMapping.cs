using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Risks.Queries.GetRiskTrackingReports;
using DigitNow.Domain.DocumentManagement.Public.Risks.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetRiskTrackingReportsMapping : Profile
    {
        public GetRiskTrackingReportsMapping()
        {
            CreateMap<GetRiskTrackingReportsRequest, GetRiskTrackingReportsQuery>();
        }
    }
}
