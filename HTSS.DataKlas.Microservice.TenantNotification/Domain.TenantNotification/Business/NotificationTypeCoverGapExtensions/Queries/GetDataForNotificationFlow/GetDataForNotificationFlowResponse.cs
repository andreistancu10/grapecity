using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers.Models;
using System.Collections.Generic;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.GetDataForNotificationFlow
{
    public class GetDataForNotificationFlowResponse
    {
        public List<NotificationTypeCoverGapExtensionGridColumn> Columns { get; set; }
        public List<NotificationTypeCoverGapExtensionGridRow> Rows { get; set; }
    }
}