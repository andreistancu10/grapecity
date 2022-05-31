using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.Notifications;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.GetNotificationById
{
    public class GetNotificationByIdConsumerMapping : Profile
    {
        public GetNotificationByIdConsumerMapping()
        {
            CreateMap<NotificationElastic, GetNotificationByIdConsumerResponse>();
        }
    }
}