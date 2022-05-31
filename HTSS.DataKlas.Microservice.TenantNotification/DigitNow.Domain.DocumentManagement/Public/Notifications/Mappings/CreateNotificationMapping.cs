using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Create;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Mappings
{
    public class CreateNotificationMapping : Profile
    {
        public CreateNotificationMapping()
        {
            CreateMap<CreateNotificationRequest, CreateNotificationCommand>();
        }
    }    
}