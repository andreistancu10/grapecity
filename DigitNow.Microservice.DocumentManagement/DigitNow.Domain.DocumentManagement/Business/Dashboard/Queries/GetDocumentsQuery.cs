using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Public.Dashboard.Models;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using System;

namespace DigitNow.Domain.DocumentManagement.Business.Dashboard.Queries;

public class GetDocumentsQuery : IQuery<GetDocumentsResponse>
{    
    public int Page { get; set; } = 1;
    public int Count { get; set; } = 10;
    public DocumentsFilter Filter { get; set; } //TODO: Use FilterModel
}

public class DocumentsFilterModel
{
    public string RegistryType { get; set; } //????

    public RegistrationNoFilterModel RegistrationNoFilter { get; set; }

    public RegistrationDateFilterModel RegistrationDateFilter { get; set; }

    public DocumentType DocumentType { get; set; }

    public DocumentCategoryFilterModel DocumentCategoryFilter { get; set; }

    public string DocumentState { get; set; }
}

public class RegistrationNoFilterModel
{
    public int From { get; set; }
    public int To { get; set; }
}

public class RegistrationDateFilterModel
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class DocumentCategoryFilterModel
{
    public int CategoryId { get; set; }
}