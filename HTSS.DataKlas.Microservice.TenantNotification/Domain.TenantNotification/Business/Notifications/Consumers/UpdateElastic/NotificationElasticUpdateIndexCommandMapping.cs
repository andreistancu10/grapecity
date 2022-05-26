using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.Consumers;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Consumers.UpdateElastic
{
    public class NotificationElasticUpdateIndexCommandMapping : Profile
    {
        public NotificationElasticUpdateIndexCommandMapping()
        {
            CreateMap<INotificationElasticUpdateIndexCommand, NotificationElastic>();
        }
    }
}