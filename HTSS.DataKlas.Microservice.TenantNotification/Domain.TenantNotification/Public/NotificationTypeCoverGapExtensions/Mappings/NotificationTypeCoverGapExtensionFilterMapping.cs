using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Models;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Mappings
{
    public class NotificationTypeCoverGapExtensionFilterMapping : Profile
    {
        public NotificationTypeCoverGapExtensionFilterMapping() => CreateMap<FilterNotificationTypeCoverGapExtensionRequest, FilterNotificationTypeCoverGapExtensionQuery>();
    }
}
