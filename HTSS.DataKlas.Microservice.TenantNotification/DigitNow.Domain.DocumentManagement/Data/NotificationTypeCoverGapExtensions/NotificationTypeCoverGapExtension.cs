using System;
using HTSS.Platform.Core.Domain;
using ShiftIn.Domain.TenantNotification.Data.NotificationTypes;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationTypeCoverGapExtensions
{
    public class NotificationTypeCoverGapExtension : Entity
    {
        public NotificationTypeCoverGapExtension()
        {
        }

        public NotificationTypeCoverGapExtension(long id)
        {
            Id = id;
        }

        public long NotificationTypeId { get; set; }

        public bool Active { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual NotificationType NotificationType { get; set; }
    }
}