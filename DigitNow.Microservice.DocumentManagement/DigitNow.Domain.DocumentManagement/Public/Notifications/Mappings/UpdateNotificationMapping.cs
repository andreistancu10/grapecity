using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Mappings
{
    public class UpdateNotificationMapping : Profile
    {
        public UpdateNotificationMapping()
        {
            CreateMap<UpdateNotificationRequest, UpdateNotificationCommand>();
        }
    }    
}


