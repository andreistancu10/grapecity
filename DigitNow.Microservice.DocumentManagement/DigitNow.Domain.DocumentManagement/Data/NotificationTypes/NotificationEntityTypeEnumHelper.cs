using DigitNow.Domain.DocumentManagement.Contracts.Notifications;

namespace DigitNow.Domain.DocumentManagement.Data.NotificationTypes
{
    public static class NotificationEntityTypeEnumHelper
    {
        public static int GetNotificationEntityTypeEnumByNotificationType(
            this NotificationTypeEnum notificationTypeEnum)
        {
            switch (notificationTypeEnum)
            {
                case NotificationTypeEnum.PendingManagerReactiveEmployeeRequest:
                case NotificationTypeEnum.PendingRequesterInformativeEmployeeRequest:
                case NotificationTypeEnum.CancelledManagerInformativeEmployeeRequest:
                case NotificationTypeEnum.CancelledRequesterInformativeEmployeeRequest:
                case NotificationTypeEnum.ApprovedManagerInformativeEmployeeRequest:
                case NotificationTypeEnum.ApprovedRequesterInformativeEmployeeRequest:
                case NotificationTypeEnum.RejectedManagerInformativeEmployeeRequest:
                case NotificationTypeEnum.RejectedRequesterInformativeEmployeeRequest:
                    return (int) NotificationEntityTypeEnum.EmployeeRequest;
                case NotificationTypeEnum.ConfiguratorSchedulingTypesNotification:
                case NotificationTypeEnum.ConfiguratorSchedulingDetailsTypesNotification:
                case NotificationTypeEnum.ConfiguratorEmployeeMobilityTypesNotification:
                    return (int) NotificationEntityTypeEnum.ConfiguratorNotification;
                case NotificationTypeEnum.CancelledRequesterInformativePlanningTeamDetail:
                case NotificationTypeEnum.ApprovedRequesterInformativePlanningTeamDetail:
                    return (int)NotificationEntityTypeEnum.PlanningTeamDetail;
                case NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledResourceRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledDivisionManagerRequestedInformativeCoverGapRequest:
                case NotificationTypeEnum.CancelledDivisionManagerRequesterInformativeCoverGapRequest:
                    return (int)NotificationEntityTypeEnum.CoverGapRequest;
                default:
                    return (int) NotificationEntityTypeEnum.NotSupported;
            }
        }
    }
}