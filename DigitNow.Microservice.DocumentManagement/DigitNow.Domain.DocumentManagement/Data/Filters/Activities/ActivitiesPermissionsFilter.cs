namespace DigitNow.Domain.DocumentManagement.Data.Filters.Activities
{
    public class ActivitiesPermissionsFilter : DataFilter
    {
        public ActivitiesUserPermissionsFilters UserPermissionsFilter { get; set; }
    }

    public class ActivitiesUserPermissionsFilters
    {
        public List<long> DepartmentIds { get; set; }
    }
}
