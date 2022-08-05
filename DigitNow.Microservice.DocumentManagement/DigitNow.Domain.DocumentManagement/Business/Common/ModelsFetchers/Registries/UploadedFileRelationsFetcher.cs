using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class UploadedFileRelationsFetcher : RelationalAggregateFetcher<UploadedFilesFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IGenericCategoryModelFetcher<UploadedFilesFetcherContext> _uploadedFilesCategoriesFetcher;
        private IModelFetcher<UserModel, UploadedFilesFetcherContext> _uploadedFilesUsersFetcher;

        public IReadOnlyList<DocumentCategoryModel> UploadedFileCategoryModels => GetItems(_uploadedFilesCategoriesFetcher);
        public IReadOnlyList<UserModel> UploadedFileUsers => GetItems(_uploadedFilesUsersFetcher);

        public UploadedFileRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void AddRemoteFetchers()
        {
            _uploadedFilesCategoriesFetcher = new GenericCategoryModelFetcher<UploadedFilesFetcherContext>(_serviceProvider);
            RemoteFetchers.Add(_uploadedFilesCategoriesFetcher);

            _uploadedFilesUsersFetcher = new UploadedFilesUsersFetcher(_serviceProvider);
            RemoteFetchers.Add(_uploadedFilesUsersFetcher);
        }
    }

    internal sealed class SpecialRegisterRelationsFetcher : RelationalAggregateFetcher<SpecialRegisterFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IGenericCategoryModelFetcher<SpecialRegisterFetcherContext> _documentCategoriesFetcher;

        public SpecialRegisterRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReadOnlyList<DocumentCategoryModel> CategoryModels => GetItems(_documentCategoriesFetcher);

        protected override void AddRemoteFetchers()
        {
            _documentCategoriesFetcher = new GenericCategoryModelFetcher<SpecialRegisterFetcherContext>(_serviceProvider);
            RemoteFetchers.Add(_documentCategoriesFetcher);
        }
    }

    internal interface IGenericCategoryModelFetcher<T> : IModelFetcher<DocumentCategoryModel, T>
        where T : ModelFetcherContext
    { }

    internal class GenericCategoryModelFetcher<T> : ModelFetcher<DocumentCategoryModel, T>, IGenericCategoryModelFetcher<T>
        where T : ModelFetcherContext
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public GenericCategoryModelFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
            _catalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
        }

        protected override async Task<List<DocumentCategoryModel>> FetchInternalAsync(T context, CancellationToken cancellationToken)
        {
            var documentTypesResponse = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var documentCategoryModels = documentTypesResponse.DocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return documentCategoryModels;
        }

        [Obsolete("This will be investigated in the future")]
        private async Task<List<DocumentCategoryModel>> FetchInternalAsync_Rpc(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var documentTypesResponse = await _catalogClient.DocumentTypes.GetDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var documentCategoryModels = documentTypesResponse.DocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return documentCategoryModels;
        }
    }
}