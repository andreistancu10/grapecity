using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Public.Reports.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Mappings
{
    public class GetReportRequestMapping : Profile
    {
        public GetReportRequestMapping()
        {
            CreateMap<GetExpiredReportRequest, GetReportQuery>()
                .ForMember(c => c.Type, opt => opt.MapFrom(src => ReportType.ExpiredDocuments));
            
            CreateMap<GetToExpireReportRequest, GetReportQuery>()
                .ForMember(c => c.Type, opt => opt.MapFrom(src => ReportType.DocumentsToExpire));
        }
    }
}