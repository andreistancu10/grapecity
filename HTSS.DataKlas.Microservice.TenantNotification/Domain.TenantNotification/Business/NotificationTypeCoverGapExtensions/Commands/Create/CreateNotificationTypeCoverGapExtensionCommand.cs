using HTSS.Platform.Core.CQRS;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Commands.Create
{
    public sealed class CreateNotificationTypeCoverGapExtensionCommand : ICommand<ResultObject>
    {
        public long NotificationTypeId { get; set; }

        public bool Active { get; set; }
    }
}