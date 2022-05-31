using ShiftIn.Domain.TenantNotification.Contracts.Notifications;
using ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationStatus;

namespace ShiftIn.Domain.TenantNotification.Client.Internal.Model
{
    internal sealed class ChangeNotificationStatusEvent : IChangeNotificationStatusEvent
    {
        public long NotificationId { get; init; }
        
        public NotificationStatusEnum NotificationStatus { get; init; }
    }
}