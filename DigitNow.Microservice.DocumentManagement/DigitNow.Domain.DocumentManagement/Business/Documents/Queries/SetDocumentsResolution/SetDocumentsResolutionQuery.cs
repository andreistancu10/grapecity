using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.SetDocumentsResolution;

public record SetDocumentsResolutionQuery : IQuery<ResultObject>
{
    public DocumentBatch Batch { get; set; }

    public DocumentResolutionType Resolution { get; set; }

    public string Remarks { get; set; }
}