using AutoMapper;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using HTSS.Platform.Core.Errors;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.Authentication.Contracts.ContactDetails.Create;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;

namespace DigitNow.Domain.DocumentManagement.Business.IncomingDocuments.Commands.Create
{
    public class CreateIncomingDocumentHandler : ICommandHandler<CreateIncomingDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IIdentityService _identityService;
        private readonly ISpecialRegisterMappingService _specialRegisterMappingService;
        private readonly IIncomingDocumentService _incomingDocumentService;
        private readonly IDocumentFileService _documentFileService;
        private readonly IMailSenderService _mailSenderService;

        public CreateIncomingDocumentHandler(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IAuthenticationClient authenticationClient,
            IIdentityService identityService,
            ISpecialRegisterMappingService specialRegisterMappingService,
            IDocumentFileService documentFileService,
            IIncomingDocumentService incomingDocumentService,
            IMailSenderService mailSenderService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authenticationClient = authenticationClient;
            _identityService = identityService;
            _documentFileService = documentFileService;
            _specialRegisterMappingService = specialRegisterMappingService;
            _incomingDocumentService = incomingDocumentService;
            _mailSenderService = mailSenderService;
        }
        
        public async Task<ResultObject> Handle(CreateIncomingDocumentCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
            {
                await CreateContactDetailsAsync(request, cancellationToken);
            }

            var headOfDepartment = await _identityService.GetHeadOfDepartmentUserAsync(request.RecipientId, cancellationToken);

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
                    SourceDestinationDepartmentId = request.RecipientId,
                    RecipientId = headOfDepartment.Id
                };
            
                await _incomingDocumentService.AddAsync(newIncomingDocument, cancellationToken);
                await _documentFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, newIncomingDocument.Document, cancellationToken);

                var newWorkflowHistoryLog = WorkflowHistoryLogFactory.Create(newIncomingDocument.Document, RecipientType.HeadOfDepartment, headOfDepartment, DocumentStatus.InWorkUnallocated);
                await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowHistoryLog, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
                await _specialRegisterMappingService.MapDocumentAsync(newIncomingDocument, cancellationToken);

                if (request.ContactDetail?.Email != null)
                {
                    await _mailSenderService.SendMail_AfterIncomingDocumentCreated(
                        new UserModel { FirstName = request.IssuerName, Email = request.ContactDetail.Email },
                        newIncomingDocument.Document.RegistrationNumber,
                        newIncomingDocument.Document.RegistrationDate,
                        cancellationToken
                        );
                }
            }
            catch (Exception ex)
            {
                return ResultObject.Error(new ErrorMessage
                {
                    Message = ex.InnerException?.Message
                });
            }

            return ResultObject.Created(newIncomingDocument.DocumentId);
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
            contactDetails.IssuerName = request.IssuerName;

            await _authenticationClient.ContactDetails.CreateAsync(new CreateContactDetailRequest
            {
                ContactDetailModel = _mapper.Map<ContactDetailModel>(contactDetails)
            }, cancellationToken);
        }
    }
}