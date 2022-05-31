using System;

namespace DigitNow.Domain.DocumentManagement.Data.Notifications
{
    public interface INotificationElastic
    {
        long Id { get; }
        
        long TenantId { get; }

        string Message { get; }

        long NotificationTypeId { get; }

        string NotificationTypeName { get; }

        string NotificationStatusName { get; }

        long NotificationStatusId { get; }

        long UserId { get; }

        string UserName { get; }

        long? FromUserId { get; }

        string FromUserName { get; }

        string FromUserProfilePhotoUrl { get; }

        long? EntityId { get; }

        long? EntityTypeId { get; }

        string EntityTypeName { get; }

        bool Seen { get; }

        bool IsInformative { get; }

        bool IsUrgent { get; }

        DateTime? SeenOn { get; }

        DateTime CreatedOn { get; }

        DateTime? ModifiedOn { get; }

        NotificationElasticReactiveSettings ReactiveSettings { get; }
    }

    public interface INotificationElasticReactiveSettings
    {
        string MicroserviceName { get; }
        
        string ApplicationConfigName { get; }
        
        string ApprovePath { get; }
        
        string RejectPath { get; }
    }

    public class NotificationElasticReactiveSettings : INotificationElasticReactiveSettings
    {
        public string MicroserviceName { get; set; }
        
        public string ApplicationConfigName { get; set; }
        
        public string ApprovePath { get; set; }
        
        public string RejectPath { get; set; }
    }
}