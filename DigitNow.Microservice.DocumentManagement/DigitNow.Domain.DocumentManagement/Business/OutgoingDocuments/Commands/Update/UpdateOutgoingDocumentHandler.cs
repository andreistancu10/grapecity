﻿using System.Collections.Generic;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.OutgoingDocuments;
using HTSS.Platform.Core.CQRS;
using HTSS.Platform.Core.Errors;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Update;
using DigitNow.Domain.DocumentManagement.Data.OutgoingConnectedDocuments;

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

        RemoveConnectedDocs(request, outgoingDocFromDb);
        AddConnectedDocs(request, outgoingDocFromDb);

        _mapper.Map(request, outgoingDocFromDb);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Ok(outgoingDocFromDb.Id);
    }

    private void AddConnectedDocs(UpdateOutgoingDocumentCommand request, OutgoingDocument outgoingDocFromDb)
    {
        List<int> outgoingDocIds = outgoingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
        IEnumerable<int> idsToAdd = request.ConnectedDocumentIds.Except(outgoingDocIds);

        if (idsToAdd.Any())
        {
            List<OutgoingDocument> connectedDocuments = _dbContext.OutgoingDocuments
                .Where(doc => idsToAdd
                    .Contains(doc.RegistrationNumber))
                .ToList();

            foreach (var doc in connectedDocuments)
            {
                outgoingDocFromDb.ConnectedDocuments
                    .Add(new OutgoingConnectedDocument { ChildOutgoingDocumentId = doc.Id, RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId });
            }
        }
    }

    private void RemoveConnectedDocs(UpdateOutgoingDocumentCommand request, OutgoingDocument outgoingDocFromDb)
    {
        List<int> outgoingDocIds = outgoingDocFromDb.ConnectedDocuments.Select(cd => cd.RegistrationNumber).ToList();
        IEnumerable<int> idsToRemove = outgoingDocIds.Except(request.ConnectedDocumentIds);

        if (idsToRemove.Any())
        {

            _dbContext.OutgoingConnectedDocuments
                .RemoveRange(outgoingDocFromDb.ConnectedDocuments
                    .Where(cd => idsToRemove
                        .Contains(cd.RegistrationNumber))
                    .ToList());
        }
    }
}