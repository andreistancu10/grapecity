namespace ShiftIn.Domain.TenantNotification.Contracts.Notifications.ChangeNotificationsStatusForEntity
{
    public interface IChangeNotificationsStatusForEntityEvent
    {
        long EntityId { get; }
        
        NotificationEntityTypeEnum EntityTypeId { get; }
        
        NotificationStatusEnum NotificationStatus { get; }
    }
}