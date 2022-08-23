using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using Microsoft.EntityFrameworkCore;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Authentication.Contracts;
using DigitNow.Domain.Authentication.Contracts.ContactDetails.Create;
using DigitNow.Domain.Authentication.Client;

namespace DigitNow.Domain.DocumentManagement.Business.OutgoingDocuments.Commands.Create
{
    public class CreateOutgoingDocumentHandler : ICommandHandler<CreateOutgoingDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IOutgoingDocumentService _outgoingDocumentService;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public CreateOutgoingDocumentHandler(DocumentManagementDbContext dbContext,
            IMapper mapper,
            IOutgoingDocumentService outgoingDocumentService,
            IAuthenticationClient authenticationClient,
            IUploadedFileService uploadedFileService,
            ICatalogAdapterClient catalogAdapterClient)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _outgoingDocumentService = outgoingDocumentService;
            _authenticationClient = authenticationClient;
            _uploadedFileService = uploadedFileService;
            _catalogAdapterClient = catalogAdapterClient;
        }

        public async Task<ResultObject> Handle(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.IdentificationNumber))
                await CreateContactDetailsAsync(request, cancellationToken);

            var newOutgoingDocument = _mapper.Map<OutgoingDocument>(request);

            await _outgoingDocumentService.AddAsync(newOutgoingDocument, cancellationToken);

            await AttachConnectedDocumentsAsync(request, newOutgoingDocument, cancellationToken);
            await _uploadedFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, newOutgoingDocument.Document, cancellationToken);

            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(request.DestinationDepartmentId, cancellationToken);

            var newWorkflowHistoryLog = new WorkflowHistoryLog
            {
                DocumentId = newOutgoingDocument.DocumentId,
                RecipientType = RecipientType.Department.Id,
                RecipientId = request.DestinationDepartmentId,
                DocumentStatus = DocumentStatus.New,
                RecipientName = $"Departamentul {department.Name}",
                DestinationDepartmentId = request.DestinationDepartmentId
            };
            await _dbContext.WorkflowHistoryLogs.AddAsync(newWorkflowHistoryLog, cancellationToken);

            await _dbContext.SingleUpdateAsync(newOutgoingDocument, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(newOutgoingDocument.DocumentId);
        }

        private async Task AttachConnectedDocumentsAsync(CreateOutgoingDocumentCommand request, OutgoingDocument
            outgoingDocumentForCreation, CancellationToken cancellationToken)
        {
            if (request.ConnectedDocumentIds.Any())
            {
                var connectedDocuments = await _dbContext.Documents
                    .Where(x => request.ConnectedDocumentIds.Contains(x.Id)).ToListAsync(cancellationToken);

                foreach (var connectedDocument in connectedDocuments)
                {
                    outgoingDocumentForCreation.ConnectedDocuments
                        .Add(new ConnectedDocument { DocumentId = connectedDocument.Id });
                }
            }
        }

        private async Task CreateContactDetailsAsync(CreateOutgoingDocumentCommand request, CancellationToken cancellationToken)
        {
            var contactDetails = request.ContactDetail;
            contactDetails.IdentificationNumber = request.IdentificationNumber;
            contactDetails.IssuerName = request.RecipientName;

            await _authenticationClient.ContactDetails.CreateAsync(new CreateContactDetailRequest
            {
                ContactDetailModel = _mapper.Map<ContactDetailModel>(contactDetails)
            }, cancellationToken);
        }
    }
}