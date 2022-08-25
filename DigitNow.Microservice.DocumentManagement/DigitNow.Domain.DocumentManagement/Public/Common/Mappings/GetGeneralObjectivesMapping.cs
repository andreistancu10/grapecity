using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.ObjectivesDashboard.Queries.GetGeneralObjectives;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;
using DigitNow.Domain.DocumentManagement.Public.ObjectivesDashboard.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Mappings
{
    public class GetGeneralObjectivesMapping: Profile
    {
        public GetGeneralObjectivesMapping()
        {
            CreateMap<GeneralObjectiveFilterDto,ObjectiveFilter>();
            {
                CreateMap<ObjectiveCreationDateFilterDto, ObjectiveCreationDateFilter>();
                CreateMap<ObjectiveTitleFilterDto, ObjectiveTitleFilter>();
                CreateMap<ObjectiveCodeFilterDto, ObjectiveCodeFilter>();
                CreateMap<ObjectiveStateFilterDto, ObjectiveStateFilter>();
            }
            CreateMap<GetGeneralObjectivesRequest, GetGeneralObjectivesQuery>()
               .ForMember(m => m.Filter, opt => opt.MapFrom(src => src.Filter ?? new GeneralObjectiveFilterDto()));
        }
    }
}
