using AutoMapper;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.Catalog.Contracts.ArchivedDocumentCategories.GetAll;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers
{
    internal sealed class GenericArchivedDocumentsCategoriesFetcher : ModelFetcher<ArchivedDocumentCategoryModel, ModelFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public GenericArchivedDocumentsCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<ArchivedDocumentCategoryModel>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            var response = await _catalogClient.ArchivedDocuments.GetArchivedDocumentCategiesAsync(new GetArchivedDocumentCategoriesRequest(), cancellationToken);

            var documentCategoryModels = response.Categories
                .Select(x => _mapper.Map<ArchivedDocumentCategoryModel>(x))
                .ToList();

            return documentCategoryModels;
        }
    }
}