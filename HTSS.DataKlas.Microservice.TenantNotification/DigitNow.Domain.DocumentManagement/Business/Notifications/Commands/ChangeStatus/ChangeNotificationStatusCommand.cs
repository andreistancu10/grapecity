using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.ChangeStatus
{
    public sealed class ChangeNotificationStatusCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        
        public long NotificationStatusId { get; set; }
    }
}