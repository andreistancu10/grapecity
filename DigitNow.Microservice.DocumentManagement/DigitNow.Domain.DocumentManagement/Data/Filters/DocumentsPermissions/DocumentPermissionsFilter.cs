using System;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    public class DocumentPermissionsFilter : DataFilter
    {
        public DocumentUserContextFilter UserContextFilter { get; set; }

        public DocumentDepartmentPermissionsFilters DepartmentsPermissionsFilter { get; set; }

        public DocumentUserPermissionsFilters UserPermissionsFilter { get; set; }
    }

    public class DocumentUserContextFilter
    {
        public long DepartmentId { get; set; }
    }

    public class DocumentDepartmentPermissionsFilters
    {
        public DocumentRegistryOfficeDepartmentRightsFilter RegistryOfficeRightsFilter { get; set; }
    }

    public class DocumentRegistryOfficeDepartmentRightsFilter
    {
        public long DepartmentId { get; set; }
    }

    public class DocumentUserPermissionsFilters
    {
        public DocumentMayorPermissionsFilter MayorPermissionsFilter { get; set; }

        public DocumentHeadOfDepartmentPermissionsFilter HeadOfDepartmentPermissionsFilter { get; set; }

        public DocumentFunctionaryPermissionsFilter FunctionaryPermissionsFilter { get; set; }
    }

    public class DocumentMayorPermissionsFilter
    {
    }

    public class DocumentHeadOfDepartmentPermissionsFilter
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
    }

    public class DocumentFunctionaryPermissionsFilter
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
    }
}
