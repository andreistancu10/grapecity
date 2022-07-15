using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Reports.Queries;
using DigitNow.Domain.DocumentManagement.Public.Reports.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Reports.Mappings;

public class GetReportRequestMapping : Profile
{
    public GetReportRequestMapping()
    {
        CreateMap<GetReportRequest, GetReportQuery>();
    }
}