using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.Notifications.Models;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Mappings
{
    public class CreateNotificationMapping : Profile
    {
        public CreateNotificationMapping()
        {
            CreateMap<CreateNotificationRequest, CreateNotificationCommand>();
        }
    }    
}