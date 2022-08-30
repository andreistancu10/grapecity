using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.SpecificObjectives.Queries.Get;
using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives;
using DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Models;
using DigitNow.Domain.DocumentManagement.Public.SpecificObjective.Models;
using DigitNow.Domain.DocumentManagement.Public.SpecificObjectives.Models;

namespace DigitNow.Domain.DocumentManagement.Public.SpecialObjective.Mappings
{
    public class SpecificObjectiveMappings : Profile
    {
        public SpecificObjectiveMappings()
        {
            CreateMap<CreateSpecificObjectiveRequest, CreateSpecificObjectiveCommand>();
            CreateMap<UpdateSpecificObjectiveRequest, UpdateSpecificObjectiveCommand>();

            CreateMap<GetSpecificObjectiveRequest, GetSpecificObjectiveQuery>()
                .ForMember(m => m.Filter, opt => opt.MapFrom(src => src.Filter ?? new SpecificObjectiveFilterDto()));

            CreateMap<SpecificObjectiveFilterDto, SpecificObjectiveFilter>()
                .ForMember(m => m.GeneralObjectiveIdFilter, opt => opt.MapFrom(src => src.GeneralObjectiveIdFilter))
                .ForMember(m => m.CodeFilter, opt => opt.MapFrom(src => src.CodeFilter))
                .ForMember(m => m.TitleFilter, opt => opt.MapFrom(src => src.TitleFilter))
                .ForMember(m => m.DepartmentFilter, opt => opt.MapFrom(src => src.DepartmentFilter))
                .ForMember(m => m.FunctionaryFilter, opt => opt.MapFrom(src => src.FunctionaryFilter))
                .ForMember(m => m.StateFilter, opt => opt.MapFrom(src => src.StateFilter));
                {
                CreateMap<SpecificObjectiveGeneralObjectiveIdFilterDto, SpecificObjectiveGeneralObjectiveIdFilter>()
                    .ForMember(m => m.ObjectiveId, opt => opt.MapFrom(src => src.ObjectiveId));
                CreateMap<SpecificObjectiveCodeFilterDto, SpecificObjectiveCodeFilter>()
                    .ForMember(m => m.Code, opt => opt.MapFrom(src => src.Code));
                CreateMap<SpecificObjectiveTitleFilterDto, SpecificObjectiveTitleFilter>()
                    .ForMember(m => m.Title, opt => opt.MapFrom(src => src.Title));
                CreateMap<SpecificObjectiveDepartmentFilterDto, SpecificObjectiveDepartmentFilter>()
                    .ForMember(m => m.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
                CreateMap<SpecificObjectiveFunctionaryFilterDto, SpecificObjectiveFunctionaryFilter>()
                    .ForMember(m => m.FunctionaryId, opt => opt.MapFrom(src => src.FunctionaryId));
                CreateMap<SpecificObjectiveStateFilterDto, SpecificObjectiveStateFilter>()
                    .ForMember(m => m.StateId, opt => opt.MapFrom(src => src.StateId));
                }
        }
    }
}
