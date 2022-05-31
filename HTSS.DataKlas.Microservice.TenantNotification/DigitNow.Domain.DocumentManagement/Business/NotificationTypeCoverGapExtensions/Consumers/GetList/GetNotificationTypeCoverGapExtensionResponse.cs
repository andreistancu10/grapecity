using ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList;
using System.Collections.Generic;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Consumers.GetList
{
    public class GetNotificationTypeCoverGapExtensionsResponse : IGetNotificationTypeCoverGapExtensionsResponse
    {
        public IList<INotificationTypeCoverGapExtensionResponse> NotificationTypeCoverGapExtensions { get; set; }
    }

    public class NotificationTypeCoverGapExtensionResponse : INotificationTypeCoverGapExtensionResponse
    {
        public long Id { get; set; }
        public long NotificationTypeId { get; set; }
        public string NotificationTypeCode { get; set; }
        public string NotificationTypeActor { get; set; }
        public long NotificationTypeActionId { get; set; }
        public string NotificationTypeStatus { get; set; }
        public string NotificationTypeMessage { get; set; }
        public bool Active { get; set; }
    }
}
