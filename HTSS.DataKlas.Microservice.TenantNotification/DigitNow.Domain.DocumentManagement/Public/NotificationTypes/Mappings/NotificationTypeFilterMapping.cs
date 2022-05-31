using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Mappings
{
    public class NotificationTypeFilterMapping : Profile
    {
        public NotificationTypeFilterMapping()
        {
            CreateMap<FilterNotificationTypeRequest, FilterNotificationTypesQuery>();
        }
    }
}