using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Create;
using DigitNow.Domain.DocumentManagement.Business.Activities.Commands.Update;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Mappings
{
    public class ActivityMappings : Profile
    {
        public ActivityMappings()
        {
            CreateMap<CreateActivityRequest, CreateActivityCommand>();
            CreateMap<UpdateActivityRequest, UpdateActivityCommand>();
            CreateMap<FilterActivitiesRequest, FilterActivitiesQuery>();
            CreateMap<ActivityFilterDto, ActivityFilter>();
            CreateMap<SpecificObjectivesFilterDto, SpecificObjectivesFilter>();
            CreateMap<ActivitiesFilterDto, ActivitiesFilter>();
            CreateMap<DepartmentsFilterDto, DepartmentsFilter>();
            CreateMap<FunctionariesFilterDto, FunctionariesFilter>();
        }
    }
}
