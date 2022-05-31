using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.Enums;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Helpers
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