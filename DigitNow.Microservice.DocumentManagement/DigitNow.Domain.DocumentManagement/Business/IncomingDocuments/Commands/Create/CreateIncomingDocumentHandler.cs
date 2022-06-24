using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext, IMapper mapper, IDocumentService service, IIdentityAdapterClient identityAdapterClient)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _service = service;
        _identityAdapterClient = identityAdapterClient;
    }

    public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var newIncomingDocument = _mapper.Map<IncomingDocument>(request);
        newIncomingDocument.RegistrationDate = DateTime.Now;

        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            CreateContactDetails(request);

        var incomingDocumentForCreation = _mapper.Map<IncomingDocument>(request);
        incomingDocumentForCreation.RegistrationDate = DateTime.Now;

        try
        {
            await AttachConnectedDocuments(request, newIncomingDocument, cancellationToken);
            await _service.AssignRegistrationNumberAsync(newIncomingDocument.DocumentId);

            newIncomingDocument.WorkflowHistory.Add(
            new WorkflowHistory()
            {
                RecipientType = (int)UserRole.HeadOfDepartment,
                RecipientId = request.RecipientId,
                Status = (int)Status.inWorkUnallocated,
                CreationDate = DateTime.Now,
                RegistrationNumber = newIncomingDocument.RegistrationNumber
            });

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return ResultObject.Error(new ErrorMessage()
            {
                Message = ex.InnerException?.Message
            });
        }

        return ResultObject.Created(newIncomingDocument.Id);
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
                    .Add(new ConnectedDocument { RegistrationNumber = doc.RegistrationNumber, DocumentType = DocumentType.Incoming, ChildDocumentId = doc.Id });
            }
        }
    }
    private async void CreateContactDetails(CreateIncomingDocumentCommand request)
    {
        var contactDetails = request.ContactDetail;
        contactDetails.IdentificationNumber = request.IdentificationNumber;

        var contactDetailDto = _mapper.Map<ContactDetailDto>(contactDetails);
        await _identityAdapterClient.CreateContactDetails(contactDetailDto);
    }
}