using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services.FileServices;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetUploadedFilesForDocumentId
{
    public class GetUploadedFilesForDocumentIdHandler : IQueryHandler<GetUploadedFilesForDocumentIdQuery, List<DocumentFileViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentFileService _documentFileService;

        private readonly DocumentFileRelationsFetcher _documentFileRelationsFetcher;

        public GetUploadedFilesForDocumentIdHandler(
            IMapper mapper,
            IServiceProvider serviceProvider,
            IDocumentFileService documentFileService)
        {
            _mapper = mapper;
            _documentFileService = documentFileService;
            _documentFileRelationsFetcher = new DocumentFileRelationsFetcher(serviceProvider);
        }

        public async Task<List<DocumentFileViewModel>> Handle(GetUploadedFilesForDocumentIdQuery request, CancellationToken cancellationToken)
        {
            var documentFileModels =
                await _documentFileService.FetchUploadedFilesForDocument(request.TargetId, cancellationToken);

            await _documentFileRelationsFetcher
                .UseDocumentFilesContext(new DocumentFilesFetcherContext(documentFileModels))
                .UseUploadedFilesContext(new UploadedFilesFetcherContext(_mapper.Map<List<UploadedFile>>(documentFileModels)))
                .TriggerFetchersAsync(cancellationToken);

            return documentFileModels.Select(file =>
                new DocumentFileAggregate
                {
                    DocumentFileModel = file,
                    Users = _documentFileRelationsFetcher.UploadedFileUsers,
                    DocumentFileMappings = _documentFileRelationsFetcher.DocumentFileMappingModels,
                    Categories = _documentFileRelationsFetcher.UploadedFileCategoryModels
                })
                .Select(aggregate => _mapper.Map<DocumentFileAggregate, DocumentFileViewModel>(aggregate))
                .ToList();
        }
    }
}