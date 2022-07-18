﻿using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentRelationsFetcher : RelationalAggregateFetcher<DocumentsFetcherContext>
    {
        private readonly IModelFetcher<UserModel, DocumentsFetcherContext> _documentsUsersFetcher;
        private readonly IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsCategoriesFetcher;
        private readonly IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsInternalCategoriesFetcher;

        public IReadOnlyList<UserModel> DocumentUsers { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories { get; private set; }

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
        {
            _documentsUsersFetcher = new DocumentsUsersFetcher(serviceProvider);
            Fetchers.Add(_documentsUsersFetcher);

            _documentsCategoriesFetcher = new DocumentsCategoriesFetcher(serviceProvider);
            Fetchers.Add(_documentsCategoriesFetcher);

            _documentsInternalCategoriesFetcher = new DocumentsInternalCategoriesFetcher(serviceProvider);
            Fetchers.Add(_documentsInternalCategoriesFetcher);
        }
    }
}
