using System.ComponentModel;

namespace DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.Enums
{
    public enum NotificationTypeCoverGapActor
    {
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.ResourceRequested")] ResourceRequested = 1,
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.ResourceRequester")] ResourceRequester = 2,
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.LocationManagerRequested")] LocationManagerRequested = 3,
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.LocationManagerRequester")] LocationManagerRequester = 4,
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.DivisionManagerRequested")] DivisionManagerRequested = 5,
        [Description("tenant-notification.notification-type-cover-gap-actor.backend.DivisionManagerRequester")] DivisionManagerRequester = 6
    }
}
