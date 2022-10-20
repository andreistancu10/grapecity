using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.ProcedureHistories.Queries.GetFilteredProcedures;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using DigitNow.Domain.DocumentManagement.Public.ProcedureHistories.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetFilteredProcedureHistoriesMapping : Profile
    {
        public GetFilteredProcedureHistoriesMapping()
        {
            CreateMap<GetFilteredProcedureHistoriesRequest, GetFilteredProcedureHistoriesQuery>()
                .ForPath(x => x.Filter.ProcedureHistoryProceduresFilter.ProcedureIds, opt => opt.MapFrom(src => src.Filter.ProceduresFilter.ProcedureIds));

            CreateMap<ProcedureHistoriesFilterDto, ProcedureHistoryFilter>();
            {
                CreateMap<ProcedureHistoryProceduresFilterDto, ProcedureHistoryProceduresFilter>();
            }
        }
    }
}
