using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationStatuses.Commands.Update
{
    public sealed class UpdateNotificationStatusCommand : ICommand<ResultObject>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}