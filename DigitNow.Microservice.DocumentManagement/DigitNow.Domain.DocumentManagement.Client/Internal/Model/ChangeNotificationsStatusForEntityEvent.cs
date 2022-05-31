using DigitNow.Domain.DocumentManagement.Contracts.Notifications;
using DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationsStatusForEntity;

namespace DigitNow.Domain.DocumentManagement.Client.Internal.Model
{
    internal sealed  class ChangeNotificationsStatusForEntityEvent : IChangeNotificationsStatusForEntityEvent
    {
        public NotificationStatusEnum NotificationStatus { get; init; }
        
        public long EntityId { get; init; }
        
        public NotificationEntityTypeEnum EntityTypeId { get; init; }
    }
}