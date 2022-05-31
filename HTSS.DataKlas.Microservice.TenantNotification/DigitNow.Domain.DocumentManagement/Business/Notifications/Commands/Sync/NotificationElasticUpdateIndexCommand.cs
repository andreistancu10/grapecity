using System;
using ShiftIn.Domain.TenantNotification.Business.Consumers;
using ShiftIn.Domain.TenantNotification.Data.Notifications;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Sync
{
    public class NotificationElasticUpdateIndexCommand : INotificationElasticUpdateIndexCommand
    {
        public long Id { get; set; }
        
        public long TenantId { get; set; }

        public string Message { get; set; }

        public long NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public string NotificationStatusName { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public long? FromUserId { get; set; }

        public string FromUserName { get; set; }

        public string FromUserProfilePhotoUrl { get; set; }

        public long? EntityId { get; set; }

        public long? EntityTypeId { get; set; }

        public string EntityTypeName { get; set; }

        public bool Seen { get; set; }

        public bool IsInformative { get; set; }

        public bool IsUrgent { get; set; }

        public DateTime? SeenOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public INotificationElasticUpdateIndexCommand.SynchronizationStrategyCode SynchronizationStrategy { get; set; }

        public NotificationElasticReactiveSettings ReactiveSettings { get; set; }
    }
}