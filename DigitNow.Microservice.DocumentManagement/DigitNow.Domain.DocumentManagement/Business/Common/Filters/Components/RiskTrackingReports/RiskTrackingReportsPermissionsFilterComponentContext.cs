using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Abstractions;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.RiskTrackingReports
{
    internal class RiskTrackingReportsPermissionsFilterComponentContext : IDataExpressionFilterComponentContext
    {
        public UserModel CurrentUser { get; set; }
    }
}
