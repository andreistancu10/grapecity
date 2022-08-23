using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.FileUsageContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadFileHandler : ICommandHandler<UploadFileCommand, DocumentFileViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

        public UploadFileHandler(IMapper mapper, IFileService fileService, IUploadedFileService uploadedFileService, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _fileService = fileService;
            _uploadedFileService = uploadedFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<DocumentFileViewModel> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var newGuid = Guid.NewGuid();
            var (relativePath, absolutePath) = await _fileService.UploadFileAsync(request.File, newGuid.ToString());
            var newUploadedFile = await _uploadedFileService.CreateAsync(request, newGuid, relativePath, absolutePath, cancellationToken);

            var uploadedFiles = new List<UploadedFile>
            {
                newUploadedFile
            };

            await _uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext { UploadedFiles = uploadedFiles })
                .TriggerFetchersAsync(cancellationToken);

            var fileViewModel = uploadedFiles.Select(file =>
                    new VirtualFileAggregate
                    {
                        UploadedFile = file,
                        Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                        Users = _uploadedFileRelationsFetcher.UploadedFileUsers,
                        DocumentFileMappings = _uploadedFileRelationsFetcher.DocumentFileMappings
                    })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, DocumentFileViewModel>(aggregate))
                .FirstOrDefault();

            return fileViewModel;
        }
    }
}