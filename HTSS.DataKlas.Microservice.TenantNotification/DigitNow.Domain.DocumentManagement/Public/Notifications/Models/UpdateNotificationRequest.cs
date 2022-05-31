namespace ShiftIn.Domain.TenantNotification.Public.Notifications.Models
{
    public class UpdateNotificationRequest
    {
        public long NotificationTypeId { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }

        public bool Seen { get; set; }
    }
}