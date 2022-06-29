﻿using AutoMapper;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;

public class CreateOutgoingDocumentHandler : ICommandHandler<CreateOutgoingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IOutgoingDocumentService _outgoingDocumentService;
    private readonly IIdentityAdapterClient _identityAdapterClient;

    public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        IDocumentService service,
        IOutgoingDocumentService outgoingDocumentService,
        IIdentityAdapterClient identityAdapterClient)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _outgoingDocumentService = outgoingDocumentService;
        _identityAdapterClient = identityAdapterClient;
    }

    public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            await CreateContactDetailsAsync(request, cancellationToken);

        var newOutgoingDocument = _mapper.Map<OutgoingDocument>(request);

        await _outgoingDocumentService.CreateAsync(newOutgoingDocument, cancellationToken);

        await AttachConnectedDocumentsAsync(request, newOutgoingDocument, cancellationToken);

        newOutgoingDocument.WorkflowHistory.Add(
            new WorkflowHistory
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = newOutgoingDocument.RecipientId,
                Status = (int)DocumentStatus.InWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newOutgoingDocument.Document.RegistrationNumber
            });

        await _dbContext.SingleUpdateAsync(newOutgoingDocument);
        await _dbContext.SaveChangesAsync();

        return ResultObject.Created(newOutgoingDocument.Id);
    }

    private async Task AttachConnectedDocumentsAsync(CreateOutgoingDocumentCommand request, OutgoingDocument
        outgoingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.OutgoingDocuments
                .Include(x => x.Document)
                .Where(x => request.ConnectedDocumentIds.Contains(x.Document.RegistrationNumber)).ToListAsync(cancellationToken);

            foreach (var connectedDocument in connectedDocuments)
            {
                outgoingDocumentForCreation.ConnectedDocuments
                    .Add(new ConnectedDocument { RegistrationNumber = connectedDocument.Document.RegistrationNumber, DocumentType = DocumentType.Outgoing });
            }
        }
    }

    private async Task CreateContactDetailsAsync(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        var contactDetails = request.ContactDetail;
        contactDetails.IdentificationNumber = request.IdentificationNumber;

        var contactDetailDto = _mapper.Map<ContactDetailDto>(contactDetails);
        await _identityAdapterClient.CreateContactDetailsAsync(contactDetailDto, cancellationToken);
    }
}