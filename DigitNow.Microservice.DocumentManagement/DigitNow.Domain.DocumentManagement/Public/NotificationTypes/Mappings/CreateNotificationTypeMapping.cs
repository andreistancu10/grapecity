using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypes.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypes.Mappings
{
    public class CreateNotificationTypeMapping : Profile
    {
        public CreateNotificationTypeMapping()
        {
            CreateMap<CreateNotificationTypeRequest, CreateNotificationTypeCommand>();
        }
    }    
}