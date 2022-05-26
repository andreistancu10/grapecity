using AutoMapper;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Consumers.GetNotificationById
{
    public class GetNotificationByIdConsumerMapping : Profile
    {
        public GetNotificationByIdConsumerMapping()
        {
            CreateMap<NotificationElastic, GetNotificationByIdConsumerResponse>();
        }
    }
}