namespace DigitNow.Domain.DocumentManagement.Data.Filters.Procedures
{
    public class ProcedurePermissionFilter: DataFilter
    {
        public ProceduresUserPermissionsFilters UserPermissionsFilter { get; set; }
    }
    public class ProceduresUserPermissionsFilters
    {
        public List<long> DepartmentIds { get; set; }
    }
}
