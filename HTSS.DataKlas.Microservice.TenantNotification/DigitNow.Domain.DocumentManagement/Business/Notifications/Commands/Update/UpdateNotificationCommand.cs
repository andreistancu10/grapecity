using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.Update
{
    public sealed class UpdateNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }

        public long NotificationTypeId { get; set; }

        public long NotificationStatusId { get; set; }

        public long UserId { get; set; }

        public long? FromUserId { get; set; }

        public long EntityId { get; set; }

        public bool Seen { get; set; }
    }
}