using HTSS.Platform.Infrastructure.Data.Abstractions;
using System;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationTypeCoverGapExtensions.Models
{
    public class FilterNotificationTypeCoverGapExtensionRequest : AbstractFilterModel
    {
        public long? Id { get; set; }

        public bool? Active { get; set; }
    }
}
