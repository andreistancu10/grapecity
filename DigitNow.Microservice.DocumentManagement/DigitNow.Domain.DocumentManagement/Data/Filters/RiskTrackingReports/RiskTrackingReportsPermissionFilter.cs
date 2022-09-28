namespace DigitNow.Domain.DocumentManagement.Data.Filters.RiskTrackingReports
{
    public class RiskTrackingReportsPermissionFilter : DataFilter
    {
        public RiskTrackingReportsUserPermissionsFilters UserPermissionsFilter { get; set; }
    }
    public class RiskTrackingReportsUserPermissionsFilters
    {
        public List<long> DepartmentIds { get; set; }
    }
}
