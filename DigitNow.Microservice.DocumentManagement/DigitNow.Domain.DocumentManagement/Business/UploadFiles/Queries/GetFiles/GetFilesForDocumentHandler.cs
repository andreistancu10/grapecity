using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesForDocumentHandler : IQueryHandler<GetFilesForDocumentQuery, List<DocumentFileViewModel>>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

        public GetFilesForDocumentHandler(
            DocumentManagementDbContext dbContext,
            IMapper mapper,
            IServiceProvider serviceProvider,
            IUploadedFileService uploadedFileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _uploadedFileService = uploadedFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<List<DocumentFileViewModel>> Handle(GetFilesForDocumentQuery forDocumentQuery, CancellationToken cancellationToken)
        {
            var uploadedFiles = await _uploadedFileService.FetchUploadedFiles(forDocumentQuery.DocumentId, cancellationToken);

            await _uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext { UploadFiles = uploadedFiles })
                .TriggerFetchersAsync(cancellationToken);

            return uploadedFiles.Select(file =>
                new VirtualFileAggregate
                {
                    UploadedFile = file,
                    Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                    Users = _uploadedFileRelationsFetcher.UploadedFileUsers
                })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, DocumentFileViewModel>(aggregate))
                .ToList();
        }
    }
}