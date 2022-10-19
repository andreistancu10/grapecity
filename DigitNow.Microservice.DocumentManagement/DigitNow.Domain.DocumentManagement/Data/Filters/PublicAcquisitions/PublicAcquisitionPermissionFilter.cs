namespace DigitNow.Domain.DocumentManagement.Data.Filters.PublicAcquisitions
{
    public class PublicAcquisitionPermissionFilter : DataFilter
    {
        public PublicAcquisitionPermissionsFilters UserPermissionsFilter { get; set; }
    }

    public class PublicAcquisitionPermissionsFilters
    {
        public List<long> UserDepartmentIds { get; set; }
        public long AllowedDepartmentId{ get; set; }
    }
}
