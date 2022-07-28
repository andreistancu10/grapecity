using AutoMapper;
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
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Dtos;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create;

public class CreateOutgoingDocumentHandler : ICommandHandler<CreateOutgoingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IOutgoingDocumentService _outgoingDocumentService;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IIdentityService _identityService;
    private readonly IUploadedFileService _uploadedFileService;

    public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        IOutgoingDocumentService outgoingDocumentService,
        IIdentityAdapterClient identityAdapterClient, 
        IIdentityService identityService,
        IUploadedFileService uploadedFileService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _outgoingDocumentService = outgoingDocumentService;
        _identityAdapterClient = identityAdapterClient;
        _identityService = identityService;
        _uploadedFileService = uploadedFileService;
    }

    public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            await CreateContactDetailsAsync(request, cancellationToken);

        var newOutgoingDocument = _mapper.Map<OutgoingDocument>(request);

        await _outgoingDocumentService.AddAsync(newOutgoingDocument, cancellationToken);

        await AttachConnectedDocumentsAsync(request, newOutgoingDocument, cancellationToken);
        await _uploadedFileService.CreateDocumentUploadedFilesAsync(request.UploadedFileIds, newOutgoingDocument.Document, cancellationToken);

        var newWorkflowHistoryLog = new WorkflowHistoryLog
        {
            DocumentId = newOutgoingDocument.DocumentId,
            RecipientType = RecipientType.Department.Id,
            RecipientId = request.RecipientId,
            DocumentStatus = DocumentStatus.New,
            RecipientName = $"Departamentul {request.RecipientId}!"
        };
        await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowHistoryLog);
        
        await _dbContext.SingleUpdateAsync(newOutgoingDocument, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Created(newOutgoingDocument.DocumentId);
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

        var contactDetailDto = _mapper.Map<IdentityContactDetail>(contactDetails);
        await _identityAdapterClient.CreateContactDetailsAsync(contactDetailDto, cancellationToken);
    }
}