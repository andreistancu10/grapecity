using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Mappings
{
    public class UpdateNotificationStatusMapping : Profile
    {
        public UpdateNotificationStatusMapping()
        {
            CreateMap<UpdateNotificationStatusRequest, UpdateNotificationStatusCommand>();
        }
    }    
}


