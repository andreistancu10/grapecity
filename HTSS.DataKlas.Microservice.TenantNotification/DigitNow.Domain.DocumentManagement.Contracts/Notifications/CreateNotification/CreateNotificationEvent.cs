using System.Collections.Generic;

namespace ShiftIn.Domain.TenantNotification.Contracts.Notifications.CreateNotification
{
    public interface ICreateNotificationEvent
    {
        NotificationTypeEnum NotificationTypeId { get; }

        long UserId { get; }

        long? FromUserId { get; }

        long EntityId { get; }

        IList<INotificationEventParam> Params { get; }

        INotificationEventReactiveSettings ReactiveSettings { get; }
    }

    public interface INotificationEventParam
    {
        string Value { get; }
        
        int Order { get; }
    }

    public interface INotificationEventReactiveSettings
    {
        string MicroserviceName { get; }
        
        string ApplicationConfigName { get; }
        
        string ApprovePath { get; }
        
        string RejectPath { get; }
    }

    public class CreateNotificationEvent : ICreateNotificationEvent
    {
        public NotificationTypeEnum NotificationTypeId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }

        public IList<INotificationEventParam> Params { get; set; }

        public INotificationEventReactiveSettings ReactiveSettings { get; set; }
    }

    public class NotificationEventParam : INotificationEventParam
    {
        public string Value { get; set; }
        
        public int Order { get; set; }
    }

    public class NotificationEventReactiveSettings : INotificationEventReactiveSettings
    {
        public string MicroserviceName { get; set; }
        
        public string ApplicationConfigName { get; set; }
        
        public string ApprovePath { get; set; }
        
        public string RejectPath { get; set; }
        
    }
}