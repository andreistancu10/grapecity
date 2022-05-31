using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Update;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Mappings
{
    public class UpdateNotificationTypeCoverGapExtensionMapping : Profile
    {
        public UpdateNotificationTypeCoverGapExtensionMapping()
        {
            CreateMap<UpdateNotificationTypeCoverGapExtensionsRequest, UpdateNotificationTypeCoverGapExtensionCommand>();
        }
    }    
}


