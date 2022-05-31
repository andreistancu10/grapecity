using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationsStatusForEntity;

namespace ShiftIn.Domain.TenantNotification.Client.Internal.Model
{
    internal sealed  class ChangeNotificationsStatusForEntityEvent : IChangeNotificationsStatusForEntityEvent
    {
        public NotificationStatusEnum NotificationStatus { get; init; }
        
        public long EntityId { get; init; }
        
        public NotificationEntityTypeEnum EntityTypeId { get; init; }
    }
}