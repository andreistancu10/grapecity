using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Create;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Mappings
{
    public class CreateNotificationTypeMapping : Profile
    {
        public CreateNotificationTypeMapping()
        {
            CreateMap<CreateNotificationTypeRequest, CreateNotificationTypeCommand>();
        }
    }    
}