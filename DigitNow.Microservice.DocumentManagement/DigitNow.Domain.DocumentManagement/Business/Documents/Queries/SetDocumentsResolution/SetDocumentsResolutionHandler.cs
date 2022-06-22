using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Queries.SetDocumentsResolution;

public class SetDocumentsResolutionHandler
    : IQueryHandler<SetDocumentsResolutionQuery, ResultObject>
{
    private readonly IMapper _mapper;
    private readonly IInternalDocumentService _internalDocumentService;
    private readonly IIncomingDocumentService _incomingDocumentService;

    public SetDocumentsResolutionHandler(IMapper mapper, 
        IInternalDocumentService internalDocumentService, 
        IIncomingDocumentService incomingDocumentService)
    {
        _mapper = mapper;
        _internalDocumentService = internalDocumentService;
        _incomingDocumentService = incomingDocumentService;
    }
    public async Task<ResultObject> Handle(SetDocumentsResolutionQuery query, CancellationToken cancellationToken)
    {
        var parallelResult = await Task.WhenAll(
            ProcessInternalDocumentsAsync(query.Batch, query.Resolution, query.Remarks, cancellationToken),
            ProcessIncomingDocumentsAsync(query.Batch, query.Resolution, query.Remarks, cancellationToken)
        );

        if (parallelResult.Any(x => !x))
        {
            throw new InvalidOperationException("Cannot set document resolution!");
        }

        return ResultObject.Ok();
    }

    private async Task<bool> ProcessInternalDocumentsAsync(DocumentBatch documentBatch, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
    {
        var x = new InternalDocument
        {
            IsUrgent = true
        };

        await _internalDocumentService.CreateAsync(x, cancellationToken);

        var internalDocumentIds = documentBatch.Documents
            .Where(x => x.DocumentType == DocumentType.Internal)
            .Select(x => x.Id)
            .ToList();

        await _internalDocumentService.SetResolutionAsync(internalDocumentIds, resolutionType, remarks, cancellationToken);

        return true;
    }

    private async Task<bool> ProcessIncomingDocumentsAsync(DocumentBatch documentBatch, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
    {
        //var x = new IncomingDocument
        //{
        //    IsUrgent = true,
        //    ContentSummary = "test",
        //    IssuerName = "test"
        //};

        //await _incomingDocumentService.CreateAsync(x, cancellationToken);

        var incomingDocumentIds = documentBatch.Documents
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .Select(x => x.Id)
            .ToList();

        await _incomingDocumentService.SetResolutionAsync(incomingDocumentIds, resolutionType, remarks, cancellationToken);
        
        return true;
    }
}