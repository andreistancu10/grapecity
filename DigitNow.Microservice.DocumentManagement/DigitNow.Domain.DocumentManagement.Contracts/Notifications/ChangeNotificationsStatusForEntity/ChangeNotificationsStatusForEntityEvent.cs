namespace DigitNow.Domain.DocumentManagement.Contracts.Notifications.ChangeNotificationsStatusForEntity
{
    public interface IChangeNotificationsStatusForEntityEvent
    {
        long EntityId { get; }
        
        NotificationEntityTypeEnum EntityTypeId { get; }
        
        NotificationStatusEnum NotificationStatus { get; }
    }
}