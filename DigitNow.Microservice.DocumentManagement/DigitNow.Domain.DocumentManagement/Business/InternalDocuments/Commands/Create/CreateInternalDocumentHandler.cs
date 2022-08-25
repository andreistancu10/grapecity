using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create
{
    public class CreateInternalDocumentHandler : ICommandHandler<CreateInternalDocumentCommand, ResultObject>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IInternalDocumentService _internalDocumentService;
        private readonly ICatalogAdapterClient _catalogAdapterClient;
        private readonly IDocumentFileService _documentFileService;

        public CreateInternalDocumentHandler(DocumentManagementDbContext dbContext,
            IMapper mapper, 
            IInternalDocumentService internalDocumentService,
            ICatalogAdapterClient catalogAdapterClient, 
            IDocumentFileService documentFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _internalDocumentService = internalDocumentService;
            _catalogAdapterClient = catalogAdapterClient;
            _documentFileService = documentFileService;
        }

        public async Task<ResultObject> Handle(CreateInternalDocumentCommand request, CancellationToken cancellationToken)
        {
            var newInternalDocument = _mapper.Map<InternalDocument>(request);

            await _internalDocumentService.AddAsync(newInternalDocument, cancellationToken);

            await _documentFileService.UpdateUploadedFilesWithTargetIdAsync(request.UploadedFileIds, newInternalDocument.Document, cancellationToken);

            var department = await _catalogAdapterClient.GetDepartmentByIdAsync(request.DestinationDepartmentId, cancellationToken);

            await _dbContext.WorkflowHistoryLogs.AddAsync(new WorkflowHistoryLog 
            { 
                DocumentId = newInternalDocument.DocumentId,
                RecipientId = request.DestinationDepartmentId, 
                RecipientType = RecipientType.Department.Id,
                DocumentStatus = DocumentStatus.New, 
                RecipientName = $"Departamentul {department.Name}" ,
                DestinationDepartmentId = request.DestinationDepartmentId
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultObject.Created(newInternalDocument.DocumentId);
        }
    }
}