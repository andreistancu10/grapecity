using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Update;
using ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Mappings
{
    public class UpdateNotificationStatusMapping : Profile
    {
        public UpdateNotificationStatusMapping()
        {
            CreateMap<UpdateNotificationStatusRequest, UpdateNotificationStatusCommand>();
        }
    }    
}


