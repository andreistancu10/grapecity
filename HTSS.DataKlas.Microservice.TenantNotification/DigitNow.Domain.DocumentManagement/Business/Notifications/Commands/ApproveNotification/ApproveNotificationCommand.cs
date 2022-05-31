using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.ApproveNotification
{
    public sealed class ApproveNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}