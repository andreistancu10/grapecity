using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal sealed class DocumentsInternalCategoriesFetcher : ModelFetcher<DocumentCategoryModel, DocumentsFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;
        private readonly ICatalogAdapterClient _catalogAdapterClient;

        public DocumentsInternalCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
            _catalogAdapterClient = serviceProvider.GetService<ICatalogAdapterClient>();
        }

        protected override async Task<List<DocumentCategoryModel>> FetchInternalAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var internalDocumentTypes = await _catalogAdapterClient.GetInternalDocumentTypesAsync(cancellationToken);

            var internalDocumentCategoryModels = internalDocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return internalDocumentCategoryModels;
        }

        [Obsolete("This will be investigated in the future")]
        private async Task<List<DocumentCategoryModel>> FetchInternalAsync_Rpc(DocumentsFetcherContext context, CancellationToken cancellationToken)
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
