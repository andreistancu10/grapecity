using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForTargetId
{
    public class GetUploadedFilesForTargetIdHandler : IQueryHandler<GetUploadedFilesForTargetIdQuery, List<FileViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedFileService _uploadedFileService;

        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

        public GetUploadedFilesForTargetIdHandler(
            IMapper mapper,
            IServiceProvider serviceProvider,
            IUploadedFileService uploadedFileService)
        {
            _mapper = mapper;
            _uploadedFileService = uploadedFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<List<FileViewModel>> Handle(GetUploadedFilesForTargetIdQuery request, CancellationToken cancellationToken)
        {
            var uploadedFiles =
                await _uploadedFileService.FetchUploadedFiles(request.TargetEntity, request.TargetId,
                    cancellationToken);

            await _uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext(uploadedFiles))
                .TriggerFetchersAsync(cancellationToken);

            return uploadedFiles.Select(file =>
                new VirtualFileAggregate
                {
                    UploadedFile = file,
                    Users = _uploadedFileRelationsFetcher.UploadedFileUsers,
                })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, FileViewModel>(aggregate))
                .ToList();
        }
    }
}