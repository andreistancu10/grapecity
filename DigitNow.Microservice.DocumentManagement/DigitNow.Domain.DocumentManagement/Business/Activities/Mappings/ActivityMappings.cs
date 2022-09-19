using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Activities;

namespace DigitNow.Domain.DocumentManagement.Business.Activities.Mappings
{
    public class ActivityMappings:Profile
    {
        public ActivityMappings()
        {
            //CreateMap<FilterActivitiesQuery,ActivityFilter>(); TODO
        }
    }
}