namespace ShiftIn.Domain.TenantNotification.Public.NotificationStatuses.Models
{
    public class CreateNotificationStatusRequest
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}