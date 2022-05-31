using System;
using HTSS.Platform.Core.Domain;

namespace ShiftIn.Domain.TenantNotification.Data.Notifications
{
    public class Notification : Entity
    {
        public Notification()
        {
        }

        public Notification(long id)
        {
            Id = id;
        }

        public string Message { get; set; }

        public long NotificationTypeId { get; set; }

        public long NotificationStatusId { get; set; }

        public long? FromUserId { get; set; }

        public long UserId { get; set; }

        public long? EntityId { get; set; }

        public long? EntityTypeId { get; set; }

        public bool Seen { get; set; }

        public string ReactiveSettings { get; set; }

        public DateTime? SeenOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}