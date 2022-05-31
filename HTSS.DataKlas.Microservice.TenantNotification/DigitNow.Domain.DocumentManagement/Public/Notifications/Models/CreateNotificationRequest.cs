namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Models
{
    public class CreateNotificationRequest
    {
        public long NotificationTypeId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }
    }
}