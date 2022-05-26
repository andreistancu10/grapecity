using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Mappings
{
    public class NotificationStatusFilterMapping : Profile
    {
        public NotificationStatusFilterMapping()
        {
            CreateMap<FilterNotificationStatusRequest, FilterNotificationStatusQuery>();
        }
    }
}