using System;
using HTSS.Platform.Core.Domain;
using ShiftIn.Domain.TenantNotification.Data.NotificationStatuses;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationTypes
{
    public class NotificationType : Entity
    {
        public NotificationType()
        {
        }

        public NotificationType(long id)
        {
            Id = id;
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool IsInformative { get; set; }

        public bool IsUrgent { get; set; }

        public bool Active { get; set; }

        public long NotificationStatusId { get; set; }

        public long EntityType { get; set; }

        public string TranslationLabel { get; set; }

        public string Expression { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual NotificationStatus NotificationStatus { get; set; }
    }
}