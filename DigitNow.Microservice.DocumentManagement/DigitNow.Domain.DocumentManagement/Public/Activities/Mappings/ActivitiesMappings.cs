using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Activities.Queries.FilterActivities;
using DigitNow.Domain.DocumentManagement.Public.Activities.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Activities.Mappings
{
    public class ActivitiesMappings: Profile
    {
        public ActivitiesMappings()
        {
            CreateMap<FilterActivitiesRequest, FilterActivitiesQuery>();
        }
    }
}
