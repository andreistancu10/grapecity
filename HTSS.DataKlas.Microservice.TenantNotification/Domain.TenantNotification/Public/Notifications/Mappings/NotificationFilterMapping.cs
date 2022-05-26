using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Mappings
{
    public class NotificationFilterMapping : Profile
    {
        public NotificationFilterMapping()
        {
            CreateMap<FilterNotificationsRequest, FilterNotificationQuery>();
        }
    }
}