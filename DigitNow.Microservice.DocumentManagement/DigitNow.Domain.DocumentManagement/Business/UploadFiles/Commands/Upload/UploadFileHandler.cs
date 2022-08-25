using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadFileHandler : ICommandHandler<UploadFileCommand, FileViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly IDocumentFileService _documentFileService;
        private readonly IServiceProvider _serviceProvider;

        public UploadFileHandler(
            IMapper mapper,
            IFileService fileService,
            IUploadedFileService uploadedFileService,
            IServiceProvider serviceProvider,
            IDocumentFileService documentFileService)
        {
            _mapper = mapper;
            _fileService = fileService;
            _uploadedFileService = uploadedFileService;
            _serviceProvider = serviceProvider;
            _documentFileService = documentFileService;
        }

        public async Task<FileViewModel> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var fileModel = _mapper.Map<FileModel>(request);
            var storedFileModel = await _fileService.UploadFileAsync(fileModel, request.File);
            UploadedFile newUploadedFile;

            if (request.TargetEntity == TargetEntity.Document)
            {
                var documentFileModel = _mapper.Map<DocumentFileModel>(storedFileModel);
                documentFileModel = await _documentFileService.CreateAsync(documentFileModel, cancellationToken);

                return await MapToDocumentFileViewModelAsync(documentFileModel, cancellationToken);
            }

            newUploadedFile = await _uploadedFileService.CreateAsync(storedFileModel, cancellationToken);
            return await MapToFileViewModelAsync(newUploadedFile, cancellationToken);
        }

        private async Task<DocumentFileViewModel> MapToDocumentFileViewModelAsync(DocumentFileModel documentFileModel, CancellationToken cancellationToken)
        {
            var documentFileRelationsFetcher = new DocumentFileRelationsFetcher(_serviceProvider);
            await documentFileRelationsFetcher
                .UseDocumentFilesContext(new DocumentFilesFetcherContext(new List<DocumentFileModel> { documentFileModel }))
                .TriggerFetchersAsync(cancellationToken);

            return _mapper.Map<DocumentFileAggregate, DocumentFileViewModel>(new DocumentFileAggregate
            {
                DocumentFileModel = documentFileModel,
                Categories = documentFileRelationsFetcher.UploadedFileCategoryModels,
                Users = documentFileRelationsFetcher.UploadedFileUsers,
            });
        }

        private async Task<FileViewModel> MapToFileViewModelAsync(UploadedFile uploadedFile, CancellationToken cancellationToken)
        {
            var uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(_serviceProvider);
            await uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext(new List<UploadedFile> { uploadedFile }))
                .TriggerFetchersAsync(cancellationToken);

            return _mapper.Map<VirtualFileAggregate, FileViewModel>(new VirtualFileAggregate
            {
                UploadedFile = uploadedFile,
                Users = uploadedFileRelationsFetcher.UploadedFileUsers,
            });
        }
    }
}