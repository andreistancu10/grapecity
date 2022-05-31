using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.Filter
{
    public class FilterNotificationTypeCoverGapExtensionMapping : Profile
    {
        public FilterNotificationTypeCoverGapExtensionMapping()
        {
            CreateMap<NotificationTypeCoverGapExtensionElastic, FilterNotificationTypeCoverGapExtensionResponse>();
        }
    }
}