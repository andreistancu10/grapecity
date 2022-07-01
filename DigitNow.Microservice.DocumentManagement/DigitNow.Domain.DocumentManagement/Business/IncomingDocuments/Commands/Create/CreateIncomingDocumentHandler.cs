using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.Errors;
using DigitNow.Adapters.MS.Identity;
using DigitNow.Adapters.MS.Identity.Poco;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;

public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IDocumentService _service;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IIncomingDocumentService _incomingDocumentService;
    private readonly ISpecialRegisterMappingService _specialRegisterMappingService;

    public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext, 
        IMapper mapper, 
        IDocumentService service, 
        IIdentityAdapterClient identityAdapterClient,
        IIncomingDocumentService incomingDocumentService, 
        ISpecialRegisterMappingService specialRegisterMappingService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
        _identityAdapterClient = identityAdapterClient;
        _incomingDocumentService = incomingDocumentService;
        _specialRegisterMappingService = specialRegisterMappingService;
    }

    public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            await CreateContactDetailsAsync(request, cancellationToken);

        var newIncomingDocument = _mapper.Map<IncomingDocument>(request);

        try
        {
            await AttachConnectedDocumentsAsync(request, newIncomingDocument, cancellationToken);
            await _service.AddDocument(new Document 
            { 
                DocumentType = DocumentType.Incoming,
                IncomingDocument = newIncomingDocument 
            }, cancellationToken);

            newIncomingDocument.WorkflowHistory.Add(
            new WorkflowHistory()
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = DocumentStatus.InWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newIncomingDocument.Document.RegistrationNumber
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _specialRegisterMappingService.MapDocumentAsync(newIncomingDocument, cancellationToken);
        }
        catch (Exception ex)
        {
            return ResultObject.Error(new ErrorMessage
            {
                Message = ex.InnerException?.Message
            });
        }

        return ResultObject.Created(newIncomingDocument.Id);
    }

    private async Task AttachConnectedDocumentsAsync(CreateIncomingDocumentCommand request, IncomingDocument incomingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.IncomingDocuments
                .Include(x => x.Document)
                .Where(x => request.ConnectedDocumentIds.Contains(x.Document.RegistrationNumber))
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var connectedDocument in connectedDocuments)
            {
                incomingDocumentForCreation.ConnectedDocuments
                    .Add(new ConnectedDocument { RegistrationNumber = connectedDocument.Document.RegistrationNumber, DocumentType = DocumentType.Incoming, ChildDocumentId = connectedDocument.Id });
            }
        }
    }

    private async Task CreateContactDetailsAsync(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var contactDetails = request.ContactDetail;
        contactDetails.IdentificationNumber = request.IdentificationNumber;

        var contactDetailDto = _mapper.Map<ContactDetailDto>(contactDetails);
        await _identityAdapterClient.CreateContactDetailsAsync(contactDetailDto, cancellationToken);
    }
}