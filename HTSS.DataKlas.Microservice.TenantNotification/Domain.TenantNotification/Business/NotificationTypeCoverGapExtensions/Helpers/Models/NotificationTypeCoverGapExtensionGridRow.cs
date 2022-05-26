using ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Queries.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftIn.Domain.TenantNotification.Business.NotificationTypeCoverGapExtensions.Helpers.Models
{
    public class NotificationTypeCoverGapExtensionGridRow
    {
        public NotificationTypeCoverGapExtensionGridRow()
        {
            this.Cells = new List<FilterNotificationTypeCoverGapExtensionResponse>();
        }

        public string RowKey { get; set; }
        public List<FilterNotificationTypeCoverGapExtensionResponse> Cells { get; set; }
    }
}
