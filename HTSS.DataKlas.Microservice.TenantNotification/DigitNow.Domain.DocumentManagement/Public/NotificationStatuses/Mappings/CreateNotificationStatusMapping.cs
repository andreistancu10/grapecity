using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Create;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Mappings
{
    public class CreateNotificationStatusMapping : Profile
    {
        public CreateNotificationStatusMapping()
        {
            CreateMap<CreateNotificationStatusRequest, CreateNotificationStatusCommand>();
        }
    }    
}