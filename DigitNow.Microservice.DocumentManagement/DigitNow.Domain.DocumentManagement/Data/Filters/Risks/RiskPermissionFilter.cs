namespace DigitNow.Domain.DocumentManagement.Data.Filters.Risks
{
    public class RiskPermissionFilter : DataFilter
    {
        public RisksUserPermissionsFilters UserPermissionsFilter { get; set; }
    }

    public class RisksUserPermissionsFilters
    {
        public List<long> DepartmentIds { get; set; }
    }
}
