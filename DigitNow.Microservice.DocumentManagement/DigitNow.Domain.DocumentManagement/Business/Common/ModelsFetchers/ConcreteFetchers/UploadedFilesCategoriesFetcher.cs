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
    internal sealed class UploadedFilesDocumentCategoriesFetcher : ModelFetcher<DocumentCategoryModel, UploadedFilesFetcherContext>
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IMapper _mapper;

        public UploadedFilesDocumentCategoriesFetcher(IServiceProvider serviceProvider)
        {
            _catalogClient = serviceProvider.GetService<ICatalogClient>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<DocumentCategoryModel>> FetchInternalAsync(UploadedFilesFetcherContext context, CancellationToken cancellationToken)
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