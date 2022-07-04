﻿using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Data.Filters.ConcreteFilters
{
    public class DocumentFilter
    {
        public DocumentRegistyTypeFilter RegistryTypeFilter { get; set; }

        public DocumentRegistrationNoFilter RegistrationNoFilter { get; set; }

        public DocumentRegistrationDateFilter RegistrationDateFilter { get; set; }

        public DocumentTypeFilter TypeFilter { get; set; }

        public DocumentCategoryFilter CategoryFilter { get; set; }

        public DocumentStatusFilter StatusFilter { get; set; }
    }

    public class DocumentRegistyTypeFilter
    {
        public string RegistryType { get; set; }
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

    public class DocumentCategoryFilter
    {
        public int CategoryId { get; set; }
    }

    public class DocumentStatusFilter
    {
        public string Status { get; set; }
    }
}
