﻿using System;
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

        public override bool IsInternal => false;

        public override async Task<IReadOnlyList<DocumentCategoryModel>> FetchAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
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
