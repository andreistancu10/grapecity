using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;

namespace DigitNow.Domain.DocumentManagement.Business.InternalDocuments.Commands.Create;

public class CreateInternalDocumentHandler : ICommandHandler<CreateInternalDocumentCommand, ResultObject>
{
    private readonly IMapper _mapper;
    private readonly IInternalDocumentService _internalDocumentService;
    private readonly IUploadedFileService _uploadedFileService;

    public CreateInternalDocumentHandler(IMapper mapper, IInternalDocumentService internalDocumentService, IUploadedFileService uploadedFileService)
    {
        _mapper = mapper;
        _internalDocumentService = internalDocumentService;
        _uploadedFileService = uploadedFileService;
    }

    public async Task<ResultObject> Handle(CreateInternalDocumentCommand request, CancellationToken cancellationToken)
    {
        var internalDocumentForCreation = _mapper.Map<InternalDocument>(request);

        await _internalDocumentService.CreateAsync(internalDocumentForCreation, cancellationToken).ConfigureAwait(false);
        await _uploadedFileService.CreateDocumentUploadedFilesAsync(request.UploadedFileIds, internalDocumentForCreation.Document, cancellationToken);

        return ResultObject.Created(internalDocumentForCreation.Id);
    }
}