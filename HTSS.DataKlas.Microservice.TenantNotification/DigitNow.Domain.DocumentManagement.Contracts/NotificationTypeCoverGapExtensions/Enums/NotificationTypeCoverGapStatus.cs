using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums
{
    public enum NotificationTypeCoverGapStatus
    {
        [Description("tenant-notification.notification-type-cover-gap-status.backend.Pending")] Pending = 1,
        [Description("tenant-notification.notification-type-cover-gap-status.backend.Approved")] Approved = 2,
        [Description("tenant-notification.notification-type-cover-gap-status.backend.Rejected")] Rejected = 3,
        [Description("tenant-notification.notification-type-cover-gap-status.backend.Cancelled")] Cancelled = 4
    }
}