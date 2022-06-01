using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Mappings
{
    public class NotificationStatusFilterMapping : Profile
    {
        public NotificationStatusFilterMapping()
        {
            CreateMap<FilterNotificationStatusRequest, FilterNotificationStatusQuery>();
        }
    }
}