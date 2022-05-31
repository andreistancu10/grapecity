using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.Filter;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Helpers.Models
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
