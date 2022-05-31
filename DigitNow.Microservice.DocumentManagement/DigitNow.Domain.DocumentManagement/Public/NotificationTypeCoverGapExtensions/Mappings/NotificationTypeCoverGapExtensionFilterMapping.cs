using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.Filter;
using DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Models;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Mappings
{
    public class NotificationTypeCoverGapExtensionFilterMapping : Profile
    {
        public NotificationTypeCoverGapExtensionFilterMapping() => CreateMap<FilterNotificationTypeCoverGapExtensionRequest, FilterNotificationTypeCoverGapExtensionQuery>();
    }
}
