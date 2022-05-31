using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Mappings
{
    public class UpdateNotificationTypeMapping : Profile
    {
        public UpdateNotificationTypeMapping()
        {
            CreateMap<UpdateNotificationTypeRequest, UpdateNotificationTypeCommand>();
        }
    }    
}


