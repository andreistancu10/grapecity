using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.CancelNotification
{
    public sealed class CancelNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}