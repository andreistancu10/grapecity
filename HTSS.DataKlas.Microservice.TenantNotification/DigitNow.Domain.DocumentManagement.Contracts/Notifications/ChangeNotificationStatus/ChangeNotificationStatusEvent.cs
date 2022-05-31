namespace ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationStatus
{
    public interface IChangeNotificationStatusEvent
    {
        long NotificationId { get; }
        
        NotificationStatusEnum NotificationStatus { get; }
    }
}