using DigitNow.Domain.DocumentManagement.Contracts.Notifications;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Helpers
{
    public static class ActiveFieldValueForNotificationTypeCoverGapExtensionHelper
    {
        public static bool GetDefaultActiveFieldValueForNotificationTypeCoverGapExtension(
            this NotificationTypeEnum notificationTypeEnum)
        {
            switch (notificationTypeEnum)
            {
                case NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest:
                    return true;
                default:
                    return false;
            }
        }
    }
}