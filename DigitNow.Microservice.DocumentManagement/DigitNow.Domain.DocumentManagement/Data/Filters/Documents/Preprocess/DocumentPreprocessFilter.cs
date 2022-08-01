﻿using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Preprocess
{
    public class DocumentPreprocessFilter : DataFilter
    {
        public DocumentRegistryTypeFilter RegistryTypeFilter { get; set; }

        public DocumentRegistrationNoFilter RegistrationNoFilter { get; set; }

        public DocumentRegistrationDateFilter RegistrationDateFilter { get; set; }

        public DocumentTypeFilter TypeFilter { get; set; }

        public DocumentStatusFilter StatusFilter { get; set; }

        public DocumentDepartmentFilter DepartmentFilter { get; set; }

        public DocumentIdentifiersFilter IdentifiersFilter { get; set; }

        public static DocumentPreprocessFilter Empty => new DocumentPreprocessFilter();
    }

    public class DocumentIdentifiersFilter
    {
        public List<long> Identifiers { get; set; }
    }

    public class DocumentRegistryTypeFilter
    {
        public List<string> RegistryTypes { get; set; }
    }

    public class DocumentRegistrationNoFilter
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    public class DocumentRegistrationDateFilter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class DocumentTypeFilter
    {
        public DocumentType DocumentType { get; set; }
    }

    public class DocumentStatusFilter
    {
        public DocumentStatus Status { get; set; }
    }

    public class DocumentDepartmentFilter
    {
        public List<long> DepartmentIds { get; set; }
    }
}
