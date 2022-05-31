using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Update;
using ShiftIn.Domain.TenantNotification.Public.Notifications.Models;

namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Mappings
{
    public class UpdateNotificationMapping : Profile
    {
        public UpdateNotificationMapping()
        {
            CreateMap<UpdateNotificationRequest, UpdateNotificationCommand>();
        }
    }    
}


