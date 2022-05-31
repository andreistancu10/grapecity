using ShiftIn.Domain.TenantNotification.Contracts.Notifications.GetNotificationById;

namespace ShiftIn.Domain.TenantNotification.Client.Internal.Model
{
    internal sealed class GetNotificationByIdRequest : IGetNotificationByIdRequest
    {
        public long Id { get; init; }
    }
}