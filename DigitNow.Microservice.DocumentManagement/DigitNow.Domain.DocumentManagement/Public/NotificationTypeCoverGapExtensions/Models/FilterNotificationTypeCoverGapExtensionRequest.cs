using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.NotificationTypeCoverGapExtensions.Models
{
    public class FilterNotificationTypeCoverGapExtensionRequest : AbstractFilterModel
    {
        public long? Id { get; set; }

        public bool? Active { get; set; }
    }
}
