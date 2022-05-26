namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Queries.Filter
{
    public class FilterNotificationStatusResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}