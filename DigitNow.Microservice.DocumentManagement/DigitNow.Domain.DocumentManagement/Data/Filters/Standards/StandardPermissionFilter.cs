namespace DigitNow.Domain.DocumentManagement.Data.Filters.Standards
{
    public class StandardPermissionFilter: DataFilter
    {
        public StandardsUserPermissionsFilters UserPermissionsFilter { get; set; }
    }
    public class StandardsUserPermissionsFilters
    {
        public List<long> DepartmentIds { get; set; }
    }
}
