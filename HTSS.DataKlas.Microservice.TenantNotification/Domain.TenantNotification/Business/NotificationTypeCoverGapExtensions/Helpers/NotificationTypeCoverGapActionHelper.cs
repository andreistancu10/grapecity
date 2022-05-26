using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers
{
    public static class NotificationTypeCoverGapActionHelper
    {
        public static int GetNotificationTypeCoverGapAction(
            this NotificationTypeEnum notificationTypeEnum)
        {
            switch (notificationTypeEnum)
            {
                case NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest:
                    return (int)NotificationTypeCoverGapAction.Reactive;
                default:
                    return (int)NotificationTypeCoverGapAction.Informative;
            }
        }
    }
}