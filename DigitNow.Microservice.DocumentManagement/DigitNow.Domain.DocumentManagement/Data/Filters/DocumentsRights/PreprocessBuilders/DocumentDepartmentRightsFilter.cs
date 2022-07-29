﻿using System;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.DocumentsRights.Preprocess
{
    public class DocumentDepartmentRightsFilter
    {
        public DocumentRegistryOfficeDepartmentFilter RegistryOfficeFilter { get; set; }
    }

    public class DocumentRegistryOfficeDepartmentFilter
    {
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
    }
}
