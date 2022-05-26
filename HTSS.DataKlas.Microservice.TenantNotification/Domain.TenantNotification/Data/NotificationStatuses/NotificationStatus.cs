using System;
using HTSS.Platform.Core.Domain;

namespace ShiftIn.Domain.TenantNotification.Data.NotificationStatuses
{
    public class NotificationStatus : Entity
    {
        public NotificationStatus()
        {
        }

        public NotificationStatus(long id)
        {
            Id = id;
        }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}