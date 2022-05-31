using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Create;
using DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Mappings
{
    public class CreateNotificationStatusMapping : Profile
    {
        public CreateNotificationStatusMapping()
        {
            CreateMap<CreateNotificationStatusRequest, CreateNotificationStatusCommand>();
        }
    }    
}