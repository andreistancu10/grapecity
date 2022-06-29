using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Documents.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Public.Documents.Models;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.SetDocumentsResolution;

public record SetDocumentsResolutionQuery : IQuery<ResultObject>
{
    public DocumentBatchModel Batch { get; set; }

    public DocumentResolutionType Resolution { get; set; }

    public string Remarks { get; set; }
}