using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Commands.Create
{
    public sealed class CreateNotificationTypeCoverGapExtensionCommand : ICommand<ResultObject>
    {
        public long NotificationTypeId { get; set; }

        public bool Active { get; set; }
    }
}