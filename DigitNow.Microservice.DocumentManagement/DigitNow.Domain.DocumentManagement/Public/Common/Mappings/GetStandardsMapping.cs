using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Standards.Queries.GetStandards;
using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;
using DigitNow.Domain.DocumentManagement.Public.Standards.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetStandardsMapping: Profile
    {
        public GetStandardsMapping()
        {
            CreateMap<GetStandardsRequest, GetStandardsQuery>()
                .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new StandardFilterDto()));

            CreateMap<StandardFilterDto, StandardFilter>();
            {
                CreateMap<TitleFilterDto, TitleFilter>();
                CreateMap<DepartmentFilterDto, DepartmentFilter>();
                CreateMap<FunctionariesFilterDto, FunctionariesFilter>();
                CreateMap<DeadlineFilterDto, DeadlineFilter>();
            }
        }
        
}
}
