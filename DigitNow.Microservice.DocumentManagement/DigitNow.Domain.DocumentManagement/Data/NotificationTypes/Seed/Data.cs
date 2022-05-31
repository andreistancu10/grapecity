using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications;

namespace DigitNow.Domain.DocumentManagement.Data.NotificationTypes.Seed
{
    public static class Data
    {
        public static IEnumerable<NotificationType> GetNotificationTypes()
        {
            return new[]
            {
                new NotificationType((long) NotificationTypeEnum.PendingRequesterInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.PendingRequesterInformativeEmployeeRequest.ToString(),
                    Name = "Pending Requester Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.PendingInformativeRequester",
                    EntityType = NotificationTypeEnum.PendingRequesterInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingManagerReactiveEmployeeRequest)
                {
                    Code = NotificationTypeEnum.PendingManagerReactiveEmployeeRequest.ToString(),
                    Name = "Pending Manager Reactive Employee Request",
                    IsInformative = false,
                    TranslationLabel = "Notification.EmployeeRequest.PendingReactiveManager",
                    EntityType = NotificationTypeEnum.PendingManagerReactiveEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledRequesterInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.CancelledRequesterInformativeEmployeeRequest.ToString(),
                    Name = "Cancelled Requester Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.CancelledInformativeRequester",
                    EntityType = NotificationTypeEnum.CancelledRequesterInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledManagerInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.CancelledManagerInformativeEmployeeRequest.ToString(),
                    Name = "Cancelled Manager Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.CancelledInformativeManager",
                    EntityType = NotificationTypeEnum.CancelledManagerInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedRequesterInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.ApprovedRequesterInformativeEmployeeRequest.ToString(),
                    Name = "Approved Requester Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.ApprovedInformativeRequester",
                    EntityType = NotificationTypeEnum.ApprovedRequesterInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedManagerInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.ApprovedManagerInformativeEmployeeRequest.ToString(),
                    Name = "Approved Manager Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.ApprovedInformativeManager",
                    EntityType = NotificationTypeEnum.ApprovedManagerInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedRequesterInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.RejectedRequesterInformativeEmployeeRequest.ToString(),
                    Name = "Rejected Requester Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.RejectedInformativeRequester",
                    EntityType = NotificationTypeEnum.RejectedRequesterInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedManagerInformativeEmployeeRequest)
                {
                    Code = NotificationTypeEnum.RejectedManagerInformativeEmployeeRequest.ToString(),
                    Name = "Rejected Manager Informative Employee Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.EmployeeRequest.RejectedInformativeManager",
                    EntityType = NotificationTypeEnum.RejectedManagerInformativeEmployeeRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },                
                new NotificationType((long) NotificationTypeEnum.ConfiguratorSchedulingTypesNotification)
                {
                    Code = NotificationTypeEnum.ConfiguratorSchedulingTypesNotification.ToString(),
                    Name = "Scheduling types settings changed in Configurator",
                    IsInformative = true,
                    TranslationLabel = "Notification.ConfiguratorNotification.SchedulingTypes",
                    EntityType = NotificationTypeEnum.ConfiguratorSchedulingTypesNotification
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },                
                new NotificationType((long) NotificationTypeEnum.ConfiguratorSchedulingDetailsTypesNotification)
                {
                    Code = NotificationTypeEnum.ConfiguratorSchedulingDetailsTypesNotification.ToString(),
                    Name = "Scheduling details types settings changed in Configurator",
                    IsInformative = true,
                    TranslationLabel = "Notification.ConfiguratorNotification.SchedulingDetailsTypes",
                    EntityType = NotificationTypeEnum.ConfiguratorSchedulingDetailsTypesNotification
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },                
                new NotificationType((long) NotificationTypeEnum.ConfiguratorEmployeeMobilityTypesNotification)
                {
                    Code = NotificationTypeEnum.ConfiguratorEmployeeMobilityTypesNotification.ToString(),
                    Name = "Employee mobility types settings changed in Configurator",
                    IsInformative = true,
                    TranslationLabel = "Notification.ConfiguratorNotification.MobilityTypes",
                    EntityType = NotificationTypeEnum.ConfiguratorEmployeeMobilityTypesNotification
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Pending Resource Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingResourceRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingResourceRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Pending Resource Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingResourceRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingResourceRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Pending Location Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingLocationManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingLocationManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Pending Location Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingLocationManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingLocationManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Pending Division Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingDivisionManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingDivisionManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Pending Division Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.PendingDivisionManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingDivisionManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest.ToString(),
                    Name = "Pending Resource Requested Reactive CoverGap Request",
                    IsInformative = false,
                    TranslationLabel = "Notification.CoverGapRequest.PendingResourceRequestedReactiveCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingResourceRequestedReactiveCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest.ToString(),
                    Name = "Pending Location Manager Requested Reactive CoverGap Request",
                    IsInformative = false,
                    TranslationLabel = "Notification.CoverGapRequest.PendingLocationManagerRequestedReactiveCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingLocationManagerRequestedReactiveCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest)
                {
                    Code = NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest.ToString(),
                    Name = "Pending Division Manager Requested Reactive CoverGap Request",
                    IsInformative = false,
                    TranslationLabel = "Notification.CoverGapRequest.PendingDivisionManagerRequestedReactiveCoverGapRequest",
                    EntityType = NotificationTypeEnum.PendingDivisionManagerRequestedReactiveCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Pending,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Approved Resource Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedResourceRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedResourceRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Approved Resource Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedResourceRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedResourceRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Approved Location Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedLocationManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedLocationManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Approved Location Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedLocationManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedLocationManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Approved Division Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedDivisionManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedDivisionManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Approved Division Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.ApprovedDivisionManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.ApprovedDivisionManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Resource Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedResourceRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedResourceRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Resource Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedResourceRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedResourceRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Location Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedLocationManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedLocationManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Location Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedLocationManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedLocationManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Division Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedDivisionManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedDivisionManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Rejected Division Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.RejectedDivisionManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.RejectedDivisionManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Rejected,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledResourceRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledResourceRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Resource Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledResourceRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledResourceRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Resource Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledResourceRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledResourceRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Location Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledLocationManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledLocationManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Location Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledLocationManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledLocationManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledDivisionManagerRequestedInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledDivisionManagerRequestedInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Division Manager Requested Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledDivisionManagerRequestedInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledDivisionManagerRequestedInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledDivisionManagerRequesterInformativeCoverGapRequest)
                {
                    Code = NotificationTypeEnum.CancelledDivisionManagerRequesterInformativeCoverGapRequest.ToString(),
                    Name = "Cancelled Division Manager Requester Informative CoverGap Request",
                    IsInformative = true,
                    TranslationLabel = "Notification.CoverGapRequest.CancelledDivisionManagerRequesterInformativeCoverGapRequest",
                    EntityType = NotificationTypeEnum.CancelledDivisionManagerRequesterInformativeCoverGapRequest
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.CancelledRequesterInformativePlanningTeamDetail)
                {
                    Code = NotificationTypeEnum.CancelledRequesterInformativePlanningTeamDetail.ToString(),
                    Name = "Cancelled Requester Informative PlanningTeamDetail",
                    IsInformative = true,
                    TranslationLabel = "Notification.PlanningTeamDetail.CancelledRequesterInformativePlanningTeamDetail",
                    EntityType = NotificationTypeEnum.CancelledRequesterInformativePlanningTeamDetail
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Cancelled,
                    Active = true
                },
                new NotificationType((long) NotificationTypeEnum.ApprovedRequesterInformativePlanningTeamDetail)
                {
                    Code = NotificationTypeEnum.ApprovedRequesterInformativePlanningTeamDetail.ToString(),
                    Name = "Approved Requester Informative PlanningTeamDetail",
                    IsInformative = true,
                    TranslationLabel = "Notification.PlanningTeamDetail.ApprovedRequesterInformativePlanningTeamDetail",
                    EntityType = NotificationTypeEnum.ApprovedRequesterInformativePlanningTeamDetail
                        .GetNotificationEntityTypeEnumByNotificationType(),
                    NotificationStatusId = (long) NotificationStatusEnum.Approved,
                    Active = true
                }
            };
        }
    }
}