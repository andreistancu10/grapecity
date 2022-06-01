using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.CancelNotification
{
    public sealed class CancelNotificationCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
    }
}