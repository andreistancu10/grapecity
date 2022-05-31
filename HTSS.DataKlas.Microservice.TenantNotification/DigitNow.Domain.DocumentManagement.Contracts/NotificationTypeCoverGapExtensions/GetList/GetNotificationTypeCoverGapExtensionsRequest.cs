using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Contracts.NotificationTypeCoverGapExtensions.GetList
{
    public interface IGetNotificationTypeCoverGapExtensionsRequest
    {
        string NotificationTypeStatus { get; }
    }
}
