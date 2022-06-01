using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ApproveNotification
{
    public sealed class ApproveNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}