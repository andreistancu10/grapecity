using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Data.Notifications.Common;
using HTSS.Platform.Infrastructure.Data.Abstractions;

namespace DigitNow.Domain.DocumentManagement.Public.Notifications.Models
{
    public class FilterNotificationsRequest : AbstractFilterModel, INotificationFilter
    {
        public long? Id { get; set; }

        public string Message { get; set; }

        public long? NotificationTypeId { get; set; }

        public List<long> NotificationTypeIds { get; set; }

        public string NotificationTypeName { get; set; }

        public string NotificationStatusName { get; set; }

        public long? NotificationStatusId { get; set; }

        public List<long> NotificationStatusIds { get; set; }

        public long? UserId { get; set; }

        public string UserName { get; set; }

        public long? FromUserId { get; set; }

        public string FromUserName { get; set; }

        public long? EntityId { get; set; }

        public long? EntityTypeId { get; set; }

        public string EntityTypeName { get; set; }

        public bool? Seen { get; set; }

        public List<long> NotificationTypeCategoryIds { get; set; }

        public bool? IsInformative { get; set; }

        public bool? IsUrgent { get; set; }

        public string SeenOn { get; set; }

        public string CreatedOn { get; set; }

        public string ModifiedOn { get; set; }
    }
}