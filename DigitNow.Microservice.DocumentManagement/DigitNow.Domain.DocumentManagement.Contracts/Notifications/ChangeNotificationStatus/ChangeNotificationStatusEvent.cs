namespace DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationStatus
{
    public interface IChangeNotificationStatusEvent
    {
        long NotificationId { get; }
        
        NotificationStatusEnum NotificationStatus { get; }
    }
}