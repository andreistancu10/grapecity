using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Procedures.Queries.GetFilteredProcedures;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using DigitNow.Domain.DocumentManagement.Public.Procedures.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetFilteredProceduresMapping: Profile
    {
        public GetFilteredProceduresMapping()
        {
            CreateMap<GetFilteredProceduresRequest, GetFilteredProceduresQuery>()
                .ForMember(x => x.Filter, opt => opt.MapFrom(src => src.Filter ?? new ProceduresFilterDto()));

            CreateMap<ProceduresFilterDto, ProcedureFilter>();
            {
                CreateMap<GeneralObjectivesFilterDto, GeneralObjectivesFilter>();
                CreateMap<SpecificObjectivesFilterDto, SpecificObjectivesFilter>();
                CreateMap<ActivitiesFilterDto, ActivitiesFilter>();
                CreateMap<ProcedureNameFilterDto, ProcedureNameFilter>();
                CreateMap<ProcedureStateFilterDto, ProcedureStateFilter>();
                CreateMap<ProcedureCategoriesFilterDto, ProcedureCategoriesFilter>();
                CreateMap<DepartmentsFilterDto, DepartmentsFilter>();
                CreateMap<FunctionaryFilterDto, FunctionaryFilter>();
                CreateMap<StartDateFilterDto, StartDateFilter>();
            }
        }
    }
}
