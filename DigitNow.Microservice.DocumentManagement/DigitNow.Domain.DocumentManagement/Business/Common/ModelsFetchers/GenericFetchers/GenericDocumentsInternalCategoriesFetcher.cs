using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericDocumentsInternalCategoriesFetcher : ModelFetcher<DocumentCategoryModel, ModelFetcherContext>        
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericDocumentsInternalCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DocumentCategoryModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var internalDocumentTypesResponse = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var internalDocumentCategoryModels = internalDocumentTypesResponse.InternalDocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return internalDocumentCategoryModels;
        }
    }
}
