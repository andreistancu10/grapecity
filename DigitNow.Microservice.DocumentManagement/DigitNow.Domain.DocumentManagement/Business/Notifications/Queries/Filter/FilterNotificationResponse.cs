using DigitNow.Domain.DocumentManagement.Data.Notifications;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Queries.Filter
{
    public class FilterNotificationResponse
    {
        public long Id { get; set; }

        public string Message { get; set; }

        public bool IsInformative { get; set; }

        public bool IsUrgent { get; set; }

        public long NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public string NotificationStatusName { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public long? FromUserId { get; set; }

        public string FromUserName { get; set; }

        public string FromUserProfilePhotoUrl { get; set; }

        public long EntityId { get; set; }

        public long EntityTypeId { get; set; }

        public string EntityTypeName { get; set; }

        public bool Seen { get; set; }

        public string SeenOn { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }

        public NotificationElasticReactiveSettings ReactiveSettings { get; set; }
    }
}