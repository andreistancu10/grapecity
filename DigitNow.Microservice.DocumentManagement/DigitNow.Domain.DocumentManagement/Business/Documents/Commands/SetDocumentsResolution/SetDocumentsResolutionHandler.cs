﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Documents.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Documents.Commands.SetDocumentsResolution;

public class SetDocumentsResolutionHandler
    : IQueryHandler<SetDocumentsResolutionQuery, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IInternalDocumentService _internalDocumentService;
    private readonly IIncomingDocumentService _incomingDocumentService;

    public SetDocumentsResolutionHandler(
        DocumentManagementDbContext dbContext,
        IMapper mapper,
        IInternalDocumentService internalDocumentService, 
        IIncomingDocumentService incomingDocumentService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _internalDocumentService = internalDocumentService;
        _incomingDocumentService = incomingDocumentService;
    }
    public async Task<ResultObject> Handle(SetDocumentsResolutionQuery query, CancellationToken cancellationToken)
    {
        var documentBatchIds = query.Batch.Documents.Select(x => x.Id);

        var foundDocuments =  await _dbContext.Documents
            .Where(x => documentBatchIds.Contains(x.Id))
            .Select(x => new Document { Id = x.Id, DocumentType = x.DocumentType })
            .ToListAsync(cancellationToken);

        var results = new List<bool>();
        results.Add(await ProcessInternalDocumentsAsync(foundDocuments, query.Resolution, query.Remarks, cancellationToken));
        results.Add(await ProcessIncomingDocumentsAsync(foundDocuments, query.Resolution, query.Remarks, cancellationToken));
        
        if (results.Any(x => !x))
        {
            throw new InvalidOperationException("Cannot set document resolution!");
        }

        return ResultObject.Ok();
    }

    private async Task<bool> ProcessInternalDocumentsAsync(IEnumerable<Document> documents, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
    {
        var foundInternalDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Internal)
            .Select(x => x.Id)
            .ToList();

        if (foundInternalDocuments.Any())
        {
            await _internalDocumentService.SetResolutionAsync(foundInternalDocuments, resolutionType, remarks, cancellationToken);
        }

        return true;
    }

    private async Task<bool> ProcessIncomingDocumentsAsync(IEnumerable<Document> documents, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
    {
        var foundIncomingDocuments = documents
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .Select(x => x.Id)
            .ToList();

        if (foundIncomingDocuments.Any())
        {
            await _incomingDocumentService.SetResolutionAsync(foundIncomingDocuments, resolutionType, remarks, cancellationToken);
        }

        return true;
    }
}