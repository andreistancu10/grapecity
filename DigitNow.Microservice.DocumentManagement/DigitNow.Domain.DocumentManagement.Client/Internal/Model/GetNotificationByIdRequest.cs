using DigitNow.Domain.DocumentManagement.Contracts.Notifications.GetNotificationById;

namespace DigitNow.Domain.DocumentManagement.Client.Internal.Model
{
    internal sealed class GetNotificationByIdRequest : IGetNotificationByIdRequest
    {
        public long Id { get; init; }
    }
}