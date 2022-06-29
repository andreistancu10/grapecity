using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Update;

public class UpdateOutgoingDocumentHandler : ICommandHandler<UpdateOutgoingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateOutgoingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ResultObject> Handle(UpdateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        var outgoingDocFromDb = await _dbContext.OutgoingDocuments.Include(cd => cd.ConnectedDocuments)
            .FirstOrDefaultAsync(doc => doc.Id == request.Id, cancellationToken);

        if (outgoingDocFromDb is null)
            return ResultObject.Error(new ErrorMessage
            {
                Message = $"Outgoing Document with id {request.Id} does not exist.",
                TranslationCode = "document-management.backend.update.validation.entityNotFound",
                Parameters = new object[] { request.Id }
            });

        var outcomingDocumentIds = outgoingDocFromDb.ConnectedDocuments
           .Where(x => x.DocumentType == DocumentType.Outgoing)
           .Select(cd => cd.RegistrationNumber)
           .ToList();

        RemoveConnectedDocumentsAsync(request, outcomingDocumentIds, outgoingDocFromDb);
        await AddConnectedDocsAsync(request, outcomingDocumentIds, outgoingDocFromDb, cancellationToken);

        _mapper.Map(request, outgoingDocFromDb);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Ok(outgoingDocFromDb.Id);
    }

    private async Task AddConnectedDocsAsync(UpdateOutgoingDocumentCommand request, IList<long> outcomingDocumentIds, OutgoingDocument dbOutcomingDocuments, CancellationToken cancellationToken)
    {
        var linksToAdd = request.ConnectedDocumentIds
            .Except(outcomingDocumentIds);

        if (!linksToAdd.Any())
            return;

        var connectedDocuments = await _dbContext.OutgoingDocuments
            .Include(x => x.Document)
            .Where(doc => linksToAdd.Contains(doc.Document.RegistrationNumber))
            .ToListAsync(cancellationToken);

        foreach (var connectedDocument in connectedDocuments)
        {
            var outcomingConnectedDocument = new ConnectedDocument()
            {
                ChildDocumentId = connectedDocument.Id,
                RegistrationNumber = connectedDocument.Document.RegistrationNumber,
                DocumentType = DocumentType.Outgoing
            };

            dbOutcomingDocuments.ConnectedDocuments.Add(outcomingConnectedDocument);
        }
        return;
    }

    private void RemoveConnectedDocumentsAsync(UpdateOutgoingDocumentCommand request, IList<long> outcommingDocumentIds, OutgoingDocument dbOutcomingDocuments)
    {
        var linksToRemove = outcommingDocumentIds.Except(request.ConnectedDocumentIds);

        if (!linksToRemove.Any())
            return;

        _dbContext.ConnectedDocuments
            .RemoveRange(dbOutcomingDocuments.ConnectedDocuments.Where(cd => linksToRemove.Contains(cd.RegistrationNumber))
            .ToList());

        return;
    }
}