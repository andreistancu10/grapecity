using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Consumers;
using DigitNow.Domain.DocumentManagement.Data.Notifications;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Consumers.UpdateElastic
{
    public class NotificationElasticUpdateIndexCommandMapping : Profile
    {
        public NotificationElasticUpdateIndexCommandMapping()
        {
            CreateMap<INotificationElasticUpdateIndexCommand, NotificationElastic>();
        }
    }
}