using System.Collections.Generic;
using AutoMapper;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;

public class UpdateIncomingDocumentHandler : ICommandHandler<UpdateIncomingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUploadedFileService _uploadedFileService;

    public UpdateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IUploadedFileService uploadedFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
    }

    public async Task<ResultObject> Handle(UpdateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var dbIncomingDocument = await _dbContext.IncomingDocuments.Include(cd => cd.ConnectedDocuments)
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken: cancellationToken);

        if (dbIncomingDocument is null)
            return ResultObject.Error(new ErrorMessage
            {
                Message = $"Incoming Document with id {request.Id} does not exist.",
                TranslationCode = "document-management.backend.update.validation.entityNotFound",
                Parameters = new object[] { request.Id }
            });

        var incomingDocumentIds = dbIncomingDocument.ConnectedDocuments
            .Where(x => x.DocumentType == DocumentType.Incoming)
            .Select(cd => cd.RegistrationNumber)
            .ToList();

        await RemoveConnectedDocumentsAsync(request, incomingDocumentIds, dbIncomingDocument, cancellationToken);
        await AddConnectedDocsAsync(request, incomingDocumentIds, dbIncomingDocument, cancellationToken);
        await _uploadedFileService.UpdateDocumentUploadedFilesAsync(request.UploadedFileIds, dbIncomingDocument.Document, cancellationToken);

        _mapper.Map(request, dbIncomingDocument);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Ok(dbIncomingDocument.Id);
    }

    private async Task<bool> AddConnectedDocsAsync(UpdateIncomingDocumentCommand request, IList<long> incomingDocumentIds, IncomingDocument dbIncomingDocuments, CancellationToken cancellationToken)
    {
        var linksToAdd = request.ConnectedDocumentIds
            .Except(incomingDocumentIds);

        if (!linksToAdd.Any())
            return true;

        var connectedDocuments = await _dbContext.IncomingDocuments
            .Include(x => x.Document)
            .Where(doc => linksToAdd.Contains(doc.Document.RegistrationNumber))
            .ToListAsync(cancellationToken);

        foreach (var connectedDocument in connectedDocuments)
        {
            var incomingConnectedDocument = new ConnectedDocument()
            {
                ChildDocumentId = connectedDocument.Id,
                RegistrationNumber = connectedDocument.Document.RegistrationNumber,
                DocumentType = DocumentType.Incoming
            };

            dbIncomingDocuments.ConnectedDocuments.Add(incomingConnectedDocument);
        }

        return true;
    }

    private Task<bool> RemoveConnectedDocumentsAsync(UpdateIncomingDocumentCommand request, IList<long> incomingDocumentIds, IncomingDocument dbIncomingDocuments, CancellationToken cancellationToken)
    {
        var linksToRemove = incomingDocumentIds.Except(request.ConnectedDocumentIds);

        if (!linksToRemove.Any())
            return Task.FromResult(true);

        _dbContext.ConnectedDocuments
            .RemoveRange(dbIncomingDocuments.ConnectedDocuments.Where(cd => linksToRemove.Contains(cd.RegistrationNumber))
            .ToList());

        return Task.FromResult(true);
}
}