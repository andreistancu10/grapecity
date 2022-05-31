using DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.GetList;

namespace DigitNow.Domain.DocumentManagement.Client.Internal.Model
{
    internal sealed class GetNotificationTypeCoverGapExtensionsRequest : IGetNotificationTypeCoverGapExtensionsRequest
    {
        public string NotificationTypeStatus { get; init; }
    }
}
