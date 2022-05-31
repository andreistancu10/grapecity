using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Contracts.NotificationTypeCoverGapExtensions.GetList
{
    public interface IGetNotificationTypeCoverGapExtensionsResponse
    {
        IList<INotificationTypeCoverGapExtensionResponse> NotificationTypeCoverGapExtensions { get; }
    }

    public interface INotificationTypeCoverGapExtensionResponse
    {
        long Id { get; }
        long NotificationTypeId { get; }
        string NotificationTypeCode { get; }
        string NotificationTypeActor { get; }
        long NotificationTypeActionId { get; }
        string NotificationTypeStatus { get; }
        string NotificationTypeMessage { get; }
        bool Active { get; }
    }
}
