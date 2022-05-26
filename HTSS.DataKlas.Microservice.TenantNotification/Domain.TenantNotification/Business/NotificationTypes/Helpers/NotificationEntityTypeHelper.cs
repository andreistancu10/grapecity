using System.Collections.Generic;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypes.Helpers
{
    public class NotificationEntityType
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public static class NotificationEntityTypeHelper
    {
        public static IEnumerable<NotificationEntityType> GetNotificationEntityTypes()
        {
            yield return new NotificationEntityType
            {
                Id = (long) NotificationEntityTypeEnum.EmployeeRequest,
                Name = nameof(NotificationEntityTypeEnum.EmployeeRequest)
            };

            yield return new NotificationEntityType
            {
                Id = (long) NotificationEntityTypeEnum.CoverGapRequest,
                Name = nameof(NotificationEntityTypeEnum.CoverGapRequest)
            };

            yield return new NotificationEntityType
            {
                Id = (long) NotificationEntityTypeEnum.PlanningTeamDetail,
                Name = nameof(NotificationEntityTypeEnum.PlanningTeamDetail)
            };

            yield return new NotificationEntityType
            {
                Id = (long) NotificationEntityTypeEnum.ConfiguratorNotification,
                Name = nameof(NotificationEntityTypeEnum.ConfiguratorNotification)
            };

            yield return new NotificationEntityType
            {
                Id = (long) NotificationEntityTypeEnum.NotSupported,
                Name = nameof(NotificationEntityTypeEnum.NotSupported)
            };
        }
    }
}
