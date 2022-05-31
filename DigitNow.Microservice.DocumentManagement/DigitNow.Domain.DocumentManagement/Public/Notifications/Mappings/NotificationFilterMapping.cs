using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Mappings
{
    public class NotificationFilterMapping : Profile
    {
        public NotificationFilterMapping()
        {
            CreateMap<FilterNotificationsRequest, FilterNotificationQuery>();
        }
    }
}