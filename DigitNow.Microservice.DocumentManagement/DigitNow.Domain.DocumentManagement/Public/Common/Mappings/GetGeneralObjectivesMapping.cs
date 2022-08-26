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
                CreateMap<ObjectiveRegistrationDateFilterDto, ObjectiveCreationDateFilter>()
                    .ForMember(m => m.CreationDate, opt => opt.MapFrom(src => src.ObjectiveRegistrationDate));
                CreateMap<ObjectiveTitleFilterDto, ObjectiveTitleFilter>()
                    .ForMember(m => m.Title, opt => opt.MapFrom(src => src.ObjectiveTitle));
                CreateMap<ObjectiveCodeFilterDto, ObjectiveCodeFilter>()
                    .ForMember(m => m.Code, opt => opt.MapFrom(src => src.ObjectiveCode));
                CreateMap<ObjectiveStateFilterDto, ObjectiveStateFilter>()
                    .ForMember(m => m.State, opt => opt.MapFrom(src => src.ObjectiveState));
            }
            CreateMap<GetGeneralObjectivesRequest, GetGeneralObjectivesQuery>()
               .ForMember(m => m.Filter, opt => opt.MapFrom(src => src.Filter ?? new GeneralObjectiveFilterDto()));
        }
    }
}
