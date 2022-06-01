using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationStatuses.Commands.Create
{
    public sealed class CreateNotificationStatusCommand : ICommand<ResultObject>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool Active { get; set; }
    }
}