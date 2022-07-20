using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

        public DocumentsInternalCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DocumentCategoryModel>> FetchInternalAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            //return Task.FromResult(new List<DocumentCategoryModel>());

			//TODO: Investigate further the RabbitMQ issue
            var internalDocumentTypesResponse = await _catalogClient.InternalDocumentTypes.GetInternalDocumentTypesAsync(cancellationToken);

            // Note: DocumentTypes is actual DocumentCategory
            var internalDocumentCategoryModels = internalDocumentTypesResponse.InternalDocumentTypes
                .Select(x => _mapper.Map<DocumentCategoryModel>(x))
                .ToList();

            return internalDocumentCategoryModels;
        }
    }
}
