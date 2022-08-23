using System.ComponentModel;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsAggregates;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries;
using DigitNow.Domain.DocumentManagement.Business.Common.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.ViewModels;
using DigitNow.Domain.DocumentManagement.Contracts.UploadedFiles.Enums;
using HTSS.Platform.Core.CQRS;

namespace DigitNow.Domain.DocumentManagement.Business.UploadFiles.Queries.GetFiles
{
    public class GetFilesForEntityHandler : IQueryHandler<GetFilesForEntityQuery, List<DocumentFileViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly IUploadedFileService _uploadedFileService;
        private readonly UploadedFileRelationsFetcher _uploadedFileRelationsFetcher;

        public GetFilesForEntityHandler(
            IMapper mapper,
            IServiceProvider serviceProvider,
            IUploadedFileService uploadedFileService)
        {
            _mapper = mapper;
            _uploadedFileService = uploadedFileService;
            _uploadedFileRelationsFetcher = new UploadedFileRelationsFetcher(serviceProvider);
        }

        public async Task<List<DocumentFileViewModel>> Handle(GetFilesForEntityQuery query, CancellationToken cancellationToken)
        {
            var uploadedFiles = query.TargetEntity switch
            {
                (int)TargetEntity.Document => await _uploadedFileService.FetchUploadedFilesForDocument(query.TargetId, cancellationToken),
                _ => throw new InvalidEnumArgumentException()
            };

            await _uploadedFileRelationsFetcher
                .UseUploadedFilesContext(new UploadedFilesFetcherContext { UploadedFiles = uploadedFiles })
                .TriggerFetchersAsync(cancellationToken);

            return uploadedFiles.Select(file =>
                new VirtualFileAggregate
                {
                    UploadedFile = file,
                    Categories = _uploadedFileRelationsFetcher.UploadedFileCategoryModels,
                    Users = _uploadedFileRelationsFetcher.UploadedFileUsers,
                    DocumentFileMappings = _uploadedFileRelationsFetcher.DocumentFileMappings
                })
                .Select(aggregate => _mapper.Map<VirtualFileAggregate, DocumentFileViewModel>(aggregate))
                .ToList();
        }
    }
}