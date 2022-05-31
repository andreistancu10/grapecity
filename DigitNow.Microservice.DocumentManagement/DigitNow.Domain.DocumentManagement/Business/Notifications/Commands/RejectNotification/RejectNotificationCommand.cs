using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.RejectNotification
{
    public sealed class RejectNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}