using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models
{
    public class FilterNotificationStatusRequest : AbstractFilterModel
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool? Active { get; set; }
    }
}