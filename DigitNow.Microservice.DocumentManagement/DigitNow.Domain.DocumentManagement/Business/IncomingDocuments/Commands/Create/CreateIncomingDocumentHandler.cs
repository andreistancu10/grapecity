﻿using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.IncomingDocuments;
using DigitNow.Domain.DocumentManagement.Data.WorkflowHistories;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data.IncomingConnectedDocuments;
using HTSS.Platform.Core.Errors;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;

public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDocumentService _service;

    public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IDocumentService service)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
    }

    public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var incomingDocumentForCreation = _mapper.Map<IncomingDocument>(request);
        incomingDocumentForCreation.CreationDate = DateTime.Now;
        try
        {
            await AttachConnectedDocuments(request, incomingDocumentForCreation, cancellationToken);
            await _service.AssignRegNumberAndSaveDocument(incomingDocumentForCreation);

            incomingDocumentForCreation.WorkflowHistory.Add(
            new WorkflowHistory()
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = incomingDocumentForCreation.RegistrationNumber
            });
        }
        catch (Exception ex)
        {
            return ResultObject.Error(new ErrorMessage()
            {
                Message = ex.InnerException.Message
            });
        }

        return ResultObject.Created(incomingDocumentForCreation.Id);
    }

    private async Task AttachConnectedDocuments(CreateIncomingDocumentCommand request, IncomingDocument incomingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.IncomingDocuments
                .Where(doc => request.ConnectedDocumentIds.Contains(doc.RegistrationNumber)).ToListAsync(cancellationToken: cancellationToken);

            foreach (var doc in connectedDocuments)
            {
                incomingDocumentForCreation.ConnectedDocuments
                    .Add(new IncomingConnectedDocument { RegistrationNumber = doc.RegistrationNumber, DocumentType = doc.DocumentTypeId, ChildIncomingDocumentId = doc.Id });
            }
        }
    }
}