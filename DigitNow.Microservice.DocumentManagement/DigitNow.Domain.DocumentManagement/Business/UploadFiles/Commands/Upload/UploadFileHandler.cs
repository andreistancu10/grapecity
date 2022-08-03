using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Commands.Upload
{
    public class UploadFileHandler : ICommandHandler<UploadFileCommand, FileViewModel>
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

        public async Task<FileViewModel> Handle(UploadFileCommand command, CancellationToken cancellationToken)
        {
            var newGuid = Guid.NewGuid();
            var (relativePath, absolutePath) = await _fileService.UploadFileAsync(command.File, newGuid.ToString());
            var newUploadedFile = await _uploadedFileService.CreateAsync(command, newGuid, relativePath, absolutePath, cancellationToken);

            if (command.DocumentId != null)
            {
                await _uploadedFileService.AssociateUploadedFileToDocumentAsync(
                     newUploadedFile.Id,
                     (long)command.DocumentId,
                     cancellationToken);
            }

            var uploadedFiles = new List<UploadedFile>
            {
                newUploadedFile
            };

            await _uploadedFileRelationsFetcher
                .TriggerFetchersAsync(new UploadedFilesFetcherContext
                {
                    UploadFiles = uploadedFiles
                }, cancellationToken);

            var fileViewModel = uploadedFiles.Select(file =>
                    new VirtualFileAggregate
                    {
                        UploadedFile = file,
                        Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                        Users = _uploadedFileRelationsFetcher.UploadedFileUsers
                    })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, FileViewModel>(aggregate))
                .FirstOrDefault();

            return fileViewModel;
        }
    }
}