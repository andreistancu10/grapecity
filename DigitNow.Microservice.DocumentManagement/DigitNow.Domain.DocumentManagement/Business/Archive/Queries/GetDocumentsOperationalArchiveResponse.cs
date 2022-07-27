using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace DigitNow.Domain.DocumentManagement.Business.Archive.Queries;

public class GetDocumentsOperationalArchiveResponse
{
    public long TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalPages { get; set; }
    public IList<DocumentViewModel> Items { get; set; }
}