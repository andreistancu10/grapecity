using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Helpers.Models;

namespace DigitNow.Domain.DocumentManagement.Business.NotificationTypeCoverGapExtensions.Queries.GetDataForNotificationFlow
{
    public class GetDataForNotificationFlowResponse
    {
        public List<NotificationTypeCoverGapExtensionGridColumn> Columns { get; set; }
        public List<NotificationTypeCoverGapExtensionGridRow> Rows { get; set; }
    }
}