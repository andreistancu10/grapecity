namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.GetList
{
    public class GetNotificationsResponse
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public long NotificationTypeId { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }

        public long EntityTypeId { get; set; }

        public bool Seen { get; set; }

        public string SeenOn { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }
    }
}