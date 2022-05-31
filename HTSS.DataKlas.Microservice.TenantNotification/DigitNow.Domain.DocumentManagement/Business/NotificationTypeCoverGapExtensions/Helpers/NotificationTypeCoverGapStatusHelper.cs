using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;
using ShiftIn.Utils.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers
{
    public static class NotificationTypeCoverGapStatusHelper
    {
        public static string GetNotificationTypeCoverGapStatus(
            this NotificationTypeEnum notificationTypeEnum)
        {
            switch (notificationTypeEnum)
            {
                case NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapStatus.Pending);
                case NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapStatus.Approved);
                case NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapStatus.Rejected);
                default:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapStatus.Cancelled);
            }
        }
    }
}