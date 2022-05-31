using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.RejectNotification
{
    public sealed class RejectNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}