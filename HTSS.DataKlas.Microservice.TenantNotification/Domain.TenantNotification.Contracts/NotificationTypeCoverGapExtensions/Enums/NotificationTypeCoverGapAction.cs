using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums
{
    public enum NotificationTypeCoverGapAction
    {
        [Description("tenant-notification.notification-type-cover-gap-action.backend.Informative")] Informative = 1,
        [Description("tenant-notification.notification-type-cover-gap-action.backend.Reactive")] Reactive = 2,
        [Description("tenant-notification.notification-type-cover-gap-action.backend.None")] None = 3
    }
}
