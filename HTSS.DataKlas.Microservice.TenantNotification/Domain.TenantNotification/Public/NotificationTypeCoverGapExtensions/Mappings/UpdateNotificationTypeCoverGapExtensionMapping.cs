using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Update;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Mappings
{
    public class UpdateNotificationTypeCoverGapExtensionMapping : Profile
    {
        public UpdateNotificationTypeCoverGapExtensionMapping()
        {
            CreateMap<UpdateNotificationTypeCoverGapExtensionsRequest, UpdateNotificationTypeCoverGapExtensionCommand>();
        }
    }    
}


