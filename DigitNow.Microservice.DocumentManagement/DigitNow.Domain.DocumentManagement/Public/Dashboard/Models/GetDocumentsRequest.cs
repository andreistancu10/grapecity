using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using System;

namespace DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;

public class GetDocumentsRequest
{
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public DocumentsFilter Filter { get; set; }
}

public class DocumentsFilter
{
    public string RegistryType { get; set; } //????

    public RegistrationNoFilter RegistrationNoFilter { get; set; }

    public RegistrationDateFilter RegistrationDateFilter { get; set; }

    public DocumentType DocumentType { get; set; }

    public DocumentCategoryFilter DocumentCategoryFilter { get; set; }

    public string DocumentState { get; set; }
}

public class RegistrationNoFilter
{
    public int From { get; set; }
    public int To { get; set; }
}

public class RegistrationDateFilter
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class DocumentCategoryFilter
{
    public int CategoryId { get; set; }
}