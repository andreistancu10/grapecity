using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Commands.Update;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypes.Mappings
{
    public class UpdateNotificationTypeMapping : Profile
    {
        public UpdateNotificationTypeMapping()
        {
            CreateMap<UpdateNotificationTypeRequest, UpdateNotificationTypeCommand>();
        }
    }    
}


