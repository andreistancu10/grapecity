using AutoMapper;
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
        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

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
            _documentFileService = documentFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<FileViewModel> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var newGuid = Guid.NewGuid();
            var (relativePath, absolutePath) = await _fileService.UploadFileAsync(request.File, newGuid.ToString());
            UploadedFile newUploadedFile;

            if (request.TargetEntity == TargetEntity.Document)
            {
                newUploadedFile = await _documentFileService.CreateAsync(request, newGuid, relativePath, absolutePath, cancellationToken);
            }
            else
            {
                newUploadedFile = await _uploadedFileService.CreateAsync(request, newGuid, relativePath, absolutePath, cancellationToken);
            }

            var uploadedFiles = new List<UploadedFile>
            {
                newUploadedFile
            };

            await _uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext { UploadedFiles = uploadedFiles })
                .TriggerFetchersAsync(cancellationToken);


            if (request.TargetEntity == TargetEntity.Document)
            {
                return MapToDocumentFileViewModel(uploadedFiles);
            }

            return MapToFileViewModel(uploadedFiles);
        }

        private DocumentFileViewModel MapToDocumentFileViewModel(List<UploadedFile> uploadedFiles)
        {
            var file = uploadedFiles.FirstOrDefault();

            return _mapper.Map<DocumentFileAggregate, DocumentFileViewModel>(new DocumentFileAggregate
            {
                UploadedFile = file,
                Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                Users = _uploadedFileRelationsFetcher.UploadedFileUsers,
                DocumentFileMappings = _uploadedFileRelationsFetcher.DocumentFileMappings
            });
        }

        private FileViewModel MapToFileViewModel(List<UploadedFile> uploadedFiles)
        {
            return uploadedFiles.Select(file =>
                    new VirtualFileAggregate
                    {
                        UploadedFile = file,
                        Users = _uploadedFileRelationsFetcher.UploadedFileUsers,
                    })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, FileViewModel>(aggregate))
                .FirstOrDefault();
        }
    }
}