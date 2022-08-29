namespace DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectivesPermissions
{
    public class SpecificObjectivePermissionsFilter : DataFilter
    {
        public ObjectivesUserPermissionsFilters UserPermissionsFilter { get; set; }
    }

    public class ObjectivesUserPermissionsFilters
    {
        public long UserId { get; set; }
        public List<long> DepartmentIds { get; set; }
    }
}
