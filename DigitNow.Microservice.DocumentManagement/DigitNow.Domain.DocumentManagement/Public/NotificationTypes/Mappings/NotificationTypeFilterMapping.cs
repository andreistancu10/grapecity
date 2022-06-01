using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Mappings
{
    public class NotificationTypeFilterMapping : Profile
    {
        public NotificationTypeFilterMapping()
        {
            CreateMap<FilterNotificationTypeRequest, FilterNotificationTypesQuery>();
        }
    }
}