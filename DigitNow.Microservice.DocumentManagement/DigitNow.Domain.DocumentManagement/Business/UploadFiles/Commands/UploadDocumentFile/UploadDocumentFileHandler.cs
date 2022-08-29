using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadDocumentFileHandler : ICommandHandler<UploadDocumentFileCommand, FileViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IDocumentFileService _documentFileService;
        private readonly IServiceProvider _serviceProvider;

        public UploadDocumentFileHandler(
            IMapper mapper,
            IFileService fileService,
            IServiceProvider serviceProvider,
            IDocumentFileService documentFileService)
        {
            _mapper = mapper;
            _fileService = fileService;
            _serviceProvider = serviceProvider;
            _documentFileService = documentFileService;
        }

        public async Task<FileViewModel> Handle(UploadDocumentFileCommand request, CancellationToken cancellationToken)
        {
            var fileModel = _mapper.Map<FileModel>(request);
            var storedFileModel = await _fileService.UploadFileAsync(fileModel, request.File);

            var documentFileModel = _mapper.Map<DocumentFileModel>(storedFileModel);
            documentFileModel = await _documentFileService.CreateAsync(documentFileModel, cancellationToken);

            return await MapToDocumentFileViewModelAsync(documentFileModel, cancellationToken);            
        }

        private async Task<DocumentFileViewModel> MapToDocumentFileViewModelAsync(DocumentFileModel documentFileModel, CancellationToken cancellationToken)
        {
            var documentFileRelationsFetcher = new DocumentFileRelationsFetcher(_serviceProvider);
            await documentFileRelationsFetcher
                .UseDocumentFilesContext(new DocumentFilesFetcherContext(new List<DocumentFileModel> { documentFileModel }))
                .UseUploadedFilesContext(new UploadedFilesFetcherContext(new List<UploadedFile> { _mapper.Map<UploadedFile>(documentFileModel) }))
                .TriggerFetchersAsync(cancellationToken);

            return _mapper.Map<DocumentFileAggregate, DocumentFileViewModel>(new DocumentFileAggregate
            {
                DocumentFileModel = documentFileModel,
                Categories = documentFileRelationsFetcher.UploadedFileCategoryModels,
                Users = documentFileRelationsFetcher.UploadedFileUsers,
                DocumentFileMappings = documentFileRelationsFetcher.DocumentFileMappingModels
            });
        }
    }
}