using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.Enums;
using ShiftIn.Utils.Helpers;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers
{
    public static class NotificationTypeCoverGapActorHelper
    {
        public static string GetNotificationTypeCoverGapActor(
            this NotificationTypeEnum notificationTypeEnum)
        {
            switch (notificationTypeEnum)
            {
                case NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.ResourceRequester);
                case NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.LocationManagerRequested);
                case NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.LocationManagerRequester);
                case NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledDivisionManagerRequestedInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.DivisionManagerRequested);
                case NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledDivisionManagerRequesterInformativeCoverGapRequest:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.DivisionManagerRequester);
                default:
                    return EnumDescriptionHelper.Get(NotificationTypeCoverGapActor.ResourceRequested);
            }
        }
    }
}