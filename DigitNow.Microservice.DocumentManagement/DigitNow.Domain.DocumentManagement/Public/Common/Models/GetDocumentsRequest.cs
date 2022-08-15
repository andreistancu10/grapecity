using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models
{
    public class GetDocumentsRequest
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public DocumentFilterDto Filter { get; set; }
    }

    public class DocumentFilterDto
    {
        public DocumentRegistryTypeFilterDto RegistryTypeFilter { get; set; }

        public DocumentRegistrationNoFilterDto RegistrationNoFilter { get; set; }

        public DocumentRegistrationDateFilterDto RegistrationDateFilter { get; set; }

        public DocumentTypeFilterDto DocumentTypeFilter { get; set; }

        public DocumentCategoryFilterDto DocumentCategoryFilter { get; set; }

        public DocumentStatusFilterDto DocumentStatusFilter { get; set; }

        public DocumentDepartmentFilterDto DocumentDepartmentFilter { get; set; }

        public DocumentIdentifiersFilterDto DocumentIdentifiersFilter { get; set; }

        public DocumentIdentificationNumberFilterDto IdentificationNumberFilter { get; set; }
    }

    public class DocumentIdentifiersFilterDto
    {
        public List<long> Identifiers { get; set; }
    }

    public class DocumentRegistryTypeFilterDto
    {
        public List<long> RegistryTypes { get; set; }
    }

    public class DocumentRegistrationNoFilterDto
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    public class DocumentRegistrationDateFilterDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }

    public class DocumentTypeFilterDto
    {
        public DocumentType DocumentType { get; set; }
    }

    public class DocumentCategoryFilterDto
    {
        public List<int> CategoryIds { get; set; }
    }

    public class DocumentIdentificationNumberFilterDto
    {
        public string IdentificationNumber { get; set; }
    }

    public class DocumentStatusFilterDto
    {
        public List<DocumentStatus> Status { get; set; }
    }

    public class DocumentDepartmentFilterDto
    {
        public List<long> DepartmentIds { get; set; }
    }
}