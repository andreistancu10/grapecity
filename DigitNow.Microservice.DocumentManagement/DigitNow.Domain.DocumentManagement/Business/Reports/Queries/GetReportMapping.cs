using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Reports.Queries;

public class GetReportMapping : Profile
{
    public GetReportMapping()
    {
        CreateMap<VirtualDocument, GetReportResponse>();
    }
}