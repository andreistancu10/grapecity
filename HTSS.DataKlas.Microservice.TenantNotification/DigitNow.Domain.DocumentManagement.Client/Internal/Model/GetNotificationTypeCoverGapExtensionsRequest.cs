using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace ShiftIn.Domain.TenantNotification.Client.Internal.Model
{
    internal sealed class GetNotificationTypeCoverGapExtensionsRequest : IGetNotificationTypeCoverGapExtensionsRequest
    {
        public string NotificationTypeStatus { get; init; }
    }
}
