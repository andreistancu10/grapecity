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
                .ForMember(m => m.CodeFilter, opt => opt.MapFrom(src => src.CodeFilter))
                .ForMember(m => m.TitleFilter, opt => opt.MapFrom(src => src.TitleFilter))
                .ForMember(m => m.DepartmentFilter, opt => opt.MapFrom(src => src.DepartmentFilter))
                .ForMember(m => m.FunctionaryFilter, opt => opt.MapFrom(src => src.FunctionaryFilter))
                .ForMember(m => m.StateFilter, opt => opt.MapFrom(src => src.StateFilter));
                {
                CreateMap<SpecialObjectiveCodeFilterDto, SpecialObjectiveCodeFilter>()
                    .ForMember(m => m.Code, opt => opt.MapFrom(src => src.Code));
                CreateMap<SpecialObjectiveTitleFilterDto, SpecialObjectiveTitleFilter>()
                    .ForMember(m => m.Title, opt => opt.MapFrom(src => src.Title));
                CreateMap<SpecialObjectiveDepartmentFilterDto, SpecialObjectiveDepartmentFilter>()
                    .ForMember(m => m.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId));
                CreateMap<SpecialObjectiveFunctionaryFilterDto, SpecialObjectiveFunctionaryFilter>()
                    .ForMember(m => m.FunctionaryId, opt => opt.MapFrom(src => src.FunctionaryId));
                CreateMap<SpecialObjectiveStateFilterDto, SpecialObjectiveStateFilter>()
                    .ForMember(m => m.StateId, opt => opt.MapFrom(src => src.StateId));
                }
        }
    }
}
