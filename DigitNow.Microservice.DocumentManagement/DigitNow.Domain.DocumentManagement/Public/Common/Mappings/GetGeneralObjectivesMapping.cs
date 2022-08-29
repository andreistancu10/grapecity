using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;
using DigitNow.Domain.DocumentManagement.Public.GeneralObjectives.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetGeneralObjectivesMapping: Profile
    {
        public GetGeneralObjectivesMapping()
        {
            CreateMap<GeneralObjectiveFilterDto,ObjectiveFilter>();
            {
                CreateMap<GeneralObjectiveRegistrationDateFilterDto, ObjectiveCreationDateFilter>()
                    .ForMember(m => m.CreationDate, opt => opt.MapFrom(src => src.GeneralObjectiveRegistrationDate));
                CreateMap<GeneralObjectiveTitleFilterDto, ObjectiveTitleFilter>()
                    .ForMember(m => m.Title, opt => opt.MapFrom(src => src.GeneralObjectiveTitle));
                CreateMap<GeneralObjectiveCodeFilterDto, ObjectiveCodeFilter>()
                    .ForMember(m => m.Code, opt => opt.MapFrom(src => src.GeneralObjectiveCode));
                CreateMap<GeneralObjectiveStateFilterDto, ObjectiveStateFilter>()
                    .ForMember(m => m.State, opt => opt.MapFrom(src => src.GeneralObjectiveState));
            }
            CreateMap<GetGeneralObjectivesRequest, GetGeneralObjectivesQuery>()
               .ForMember(m => m.Filter, opt => opt.MapFrom(src => src.Filter ?? new GeneralObjectiveFilterDto()));
        }
    }
}
