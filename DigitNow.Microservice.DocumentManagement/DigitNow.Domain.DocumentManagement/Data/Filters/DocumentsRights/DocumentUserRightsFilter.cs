using System;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights
{
    public class DocumentUserRightsFilter : DataFilter
    {
        public DocumentMayorRightFilter MayorRightFilter { get; set; }

        public DocumentHeadOfDepartmentRightFilter HeadOfDepartmentRightsFilter { get; set; }

        public DocumentFunctionaryRightFilter FunctionaryRightsFilter { get; set; }
    }

    public class DocumentMayorRightFilter
    {
    }

    public class DocumentHeadOfDepartmentRightFilter
    {
        public long DepartmentId { get; set; }
    }

    public class DocumentFunctionaryRightFilter
    {
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
    }
}
