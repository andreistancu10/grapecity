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
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create;

public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly ISpecialRegisterMappingService _specialRegisterMappingService;
    private readonly IIncomingDocumentService _incomingDocumentService;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IMailSenderService _mailSenderService;

    public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext,
        IMapper mapper,
        IIdentityAdapterClient identityAdapterClient,
        ISpecialRegisterMappingService specialRegisterMappingService,
        IUploadedFileService uploadedFileService,
        IIncomingDocumentService incomingDocumentService,
        IMailSenderService mailSenderService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _identityAdapterClient = identityAdapterClient;
        _uploadedFileService = uploadedFileService;
        _specialRegisterMappingService = specialRegisterMappingService;
        _incomingDocumentService = incomingDocumentService;
        _mailSenderService = mailSenderService;
    }
        
    public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            await CreateContactDetailsAsync(request, cancellationToken);

        var headOfDepartment = await GetHeadOfDepartmentUserAsync(request.RecipientId, cancellationToken);
        if (headOfDepartment == null)
            return ResultObject.Error(new ErrorMessage
            {
                Message = $"The responsible for department with id '{request.RecipientId}' was not found.",
                TranslationCode = "catalog.headOfdepartment.backend.update.validation.entityNotFound",
                Parameters = new object[] { request.RecipientId }
            });


        var newIncomingDocument = _mapper.Map<IncomingDocument>(request);

        try
        {
            await AttachConnectedDocumentsAsync(request, newIncomingDocument, cancellationToken);

            newIncomingDocument.Document = new Document
            {
                DocumentType = DocumentType.Incoming,
                IncomingDocument = newIncomingDocument,
                DestinationDepartmentId = request.RecipientId,
                RecipientId = headOfDepartment.Id
            };
            
            await _incomingDocumentService.AddAsync(newIncomingDocument, cancellationToken);
            await _uploadedFileService.CreateDocumentUploadedFilesAsync(request.UploadedFileIds, newIncomingDocument.Document, cancellationToken);

            var newWorkflowHistroryLog = WorkflowHistoryLogFactory.Create(newIncomingDocument.DocumentId, RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.InWorkUnallocated);
            await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowHistroryLog, cancellationToken);

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

        if(request.ContactDetail?.Email != null)
        {
            await _mailSenderService.SendMail_CreateIncomingDocument(
                new User { FirstName = request.IssuerName, Email = request.ContactDetail.Email },
                newIncomingDocument.Document.RegistrationNumber, 
                newIncomingDocument.Document.RegistrationDate, 
                cancellationToken
                );
        }

        return ResultObject.Created(newIncomingDocument.DocumentId);
    }

    private async Task<User> GetHeadOfDepartmentUserAsync(long departmentId, CancellationToken token)
    {
        var response = await _identityAdapterClient.GetUsersAsync(token);
        var departmentUsers = response.Users.Where(x => x.Departments.Contains(departmentId));
        return departmentUsers.FirstOrDefault();
       // return departmentUsers.FirstOrDefault(x => x.Roles.Contains(RecipientType.HeadOfDepartment.Code));
    }

    private async Task AttachConnectedDocumentsAsync(CreateIncomingDocumentCommand request, IncomingDocument incomingDocumentForCreation, CancellationToken cancellationToken)
    {
        if (request.ConnectedDocumentIds.Any())
        {
            var connectedDocuments = await _dbContext.Documents
                .Where(x => request.ConnectedDocumentIds.Contains(x.Id))
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var connectedDocument in connectedDocuments)
            {
                incomingDocumentForCreation.ConnectedDocuments
                    .Add(new ConnectedDocument { DocumentId = connectedDocument.Id });
            }
        }
    }

    private async Task CreateContactDetailsAsync(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
    {
        var contactDetails = request.ContactDetail;
        contactDetails.IdentificationNumber = request.IdentificationNumber;

        var contactDetailDto = _mapper.Map<IdentityContactDetail>(contactDetails);
        await _identityAdapterClient.CreateContactDetailsAsync(contactDetailDto, cancellationToken);
    }
}