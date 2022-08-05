using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
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