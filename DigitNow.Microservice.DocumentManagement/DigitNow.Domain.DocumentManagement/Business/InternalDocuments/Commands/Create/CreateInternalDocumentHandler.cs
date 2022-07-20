using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Adapters.MS.Identity;
using System.Linq;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using HTSS.Platform.Core.Errors;
using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Data;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;

public class CreateInternalDocumentHandler : ICommandHandler<CreateInternalDocumentCommand, ResultObject>
{
    private readonly DocumentManagementDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUploadedFileService _uploadedFileService;
    private readonly IIdentityAdapterClient _identityAdapterClient;
    private readonly IDocumentService _documentService;

    public CreateInternalDocumentHandler(DocumentManagementDbContext dbContext,
        IMapper mapper, 
        IUploadedFileService uploadedFileService, 
        IIdentityAdapterClient identityAdapterClient,
        IDocumentService documentService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _uploadedFileService = uploadedFileService;
        _identityAdapterClient = identityAdapterClient;
        _documentService = documentService;
    }

    public async Task<ResultObject> Handle(CreateInternalDocumentCommand request, CancellationToken cancellationToken)
    {
        var internalDocumentForCreation = _mapper.Map<InternalDocument>(request);

        var newDocument = new Document
        {
            DocumentType = DocumentType.Internal,
            InternalDocument = internalDocumentForCreation,
            RecipientId = request.ReceiverDepartmentId,
            Status = DocumentStatus.New,
            RecipientIsDepartment = true
        };

        await _documentService.AddDocument(newDocument, cancellationToken);
        await _uploadedFileService.CreateDocumentUploadedFilesAsync(request.UploadedFileIds, internalDocumentForCreation.Document, cancellationToken);

        internalDocumentForCreation.WorkflowHistory.Add(new WorkflowHistory { RecipientId = request.ReceiverDepartmentId, RecipientType = RecipientType.Department.Id, Status = DocumentStatus.New, RecipientName = $"Departamentul {request.ReceiverDepartmentId}" });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultObject.Created(internalDocumentForCreation.DocumentId);
    }
}