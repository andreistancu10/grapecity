using System;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    public class DocumentDepartmentRightsFilter
    {
        public DocumentRegistryOfficeDepartmentFilter RegistryOfficeFilter { get; set; }
    }

    public class DocumentRegistryOfficeDepartmentFilter
    {
        public long DepartmentId { get; set; }
    }
}
