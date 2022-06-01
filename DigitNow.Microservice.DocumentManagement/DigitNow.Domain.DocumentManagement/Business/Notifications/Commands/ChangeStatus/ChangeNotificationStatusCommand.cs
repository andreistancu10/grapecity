using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Notifications.Commands.ChangeStatus
{
    public sealed class ChangeNotificationStatusCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        
        public long NotificationStatusId { get; set; }
    }
}