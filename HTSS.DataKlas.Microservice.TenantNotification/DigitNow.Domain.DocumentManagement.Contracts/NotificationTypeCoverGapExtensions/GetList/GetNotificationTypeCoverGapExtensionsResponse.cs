using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList
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
