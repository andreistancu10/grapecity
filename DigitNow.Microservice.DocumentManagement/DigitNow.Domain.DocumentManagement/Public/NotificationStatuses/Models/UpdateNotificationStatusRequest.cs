namespace DigitNow.Domain.DocumentManagement.Public.NotificationStatuses.Models
{
    public class UpdateNotificationStatusRequest
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}