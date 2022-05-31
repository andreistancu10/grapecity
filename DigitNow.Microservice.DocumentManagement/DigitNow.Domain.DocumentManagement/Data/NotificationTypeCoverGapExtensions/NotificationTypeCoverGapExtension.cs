using System;
using DigitNow.Domain.DocumentManagement.Data.NotificationTypes;
using HTSS.Platform.Core.Domain;

namespace DigitNow.Domain.DocumentManagement.Data.NotificationTypeCoverGapExtensions
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