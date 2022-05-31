using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationStatus;

namespace DigitNow.Domain.DocumentManagement.Client.Internal.Model
{
    internal sealed class ChangeNotificationStatusEvent : IChangeNotificationStatusEvent
    {
        public long NotificationId { get; init; }
        
        public NotificationStatusEnum NotificationStatus { get; init; }
    }
}