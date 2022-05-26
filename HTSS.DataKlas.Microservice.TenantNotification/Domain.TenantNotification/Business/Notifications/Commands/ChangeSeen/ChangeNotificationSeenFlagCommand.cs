using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.Notifications.Commands.ChangeSeen
{
    public sealed class ChangeNotificationSeenFlagCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }
        
        public bool Seen { get; set; }
    }
}