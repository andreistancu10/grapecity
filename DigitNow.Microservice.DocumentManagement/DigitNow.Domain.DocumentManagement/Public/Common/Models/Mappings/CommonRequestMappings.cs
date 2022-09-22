using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Actions;
using DigitNow.Domain.DocumentManagement.Data.Filters.Activities;
using DigitNow.Domain.DocumentManagement.Data.Filters.Common;
using DigitNow.Domain.DocumentManagement.Public.Actions.Models;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;
using DigitNow.Domain.DocumentManagement.Public.Common.Models.FilterDtos;

namespace DigitNow.Domain.DocumentManagement.Public.Common.Models.Mappings
{
    public class CommonRequestMappings : Profile
    {
        public CommonRequestMappings()
        {
            CreateMap<ContactDetailsRequest, ContactDetailDto>();
            CreateMap<ActivityFilterDto, ActivityFilter>();
            CreateMap<SpecificObjectivesFilterDto, SpecificObjectivesFilter>();
            CreateMap<ActivitiesFilterDto, ActivitiesFilter>();
            CreateMap<DepartmentsFilterDto, DepartmentsFilter>();
            CreateMap<FunctionariesFilterDto, FunctionariesFilter>();
            CreateMap<ActionsFilterDto, ActionFilter>();
        }
    }
}