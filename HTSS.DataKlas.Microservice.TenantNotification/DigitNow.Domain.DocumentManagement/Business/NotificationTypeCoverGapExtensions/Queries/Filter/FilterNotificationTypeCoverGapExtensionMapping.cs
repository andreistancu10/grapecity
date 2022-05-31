using System.Globalization;
using AutoMapper;
using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.Filter;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions;

namespace ShiftIn.Domain.TenantNotificationTypeCoverGapExtension.Business.NotificationTypeCoverGapExtensions.Queries.Filter
{
    public class FilterNotificationTypeCoverGapExtensionMapping : Profile
    {
        public FilterNotificationTypeCoverGapExtensionMapping()
        {
            CreateMap<NotificationTypeCoverGapExtensionElastic, FilterNotificationTypeCoverGapExtensionResponse>();
        }
    }
}