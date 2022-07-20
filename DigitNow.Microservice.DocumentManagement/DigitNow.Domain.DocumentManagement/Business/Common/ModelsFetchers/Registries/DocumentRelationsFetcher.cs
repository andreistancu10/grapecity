using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentRelationsFetcher : RelationalAggregateFetcher<DocumentsFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IModelFetcher<UserModel, DocumentsFetcherContext> _documentsUsersFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsCategoriesFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsInternalCategoriesFetcher;

        public IReadOnlyList<UserModel> DocumentUsers => GetItems(_documentsUsersFetcher);
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories => GetItems(_documentsCategoriesFetcher);
        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories => GetItems(_documentsInternalCategoriesFetcher);

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void AddRemoteFetchers()
        {
            _documentsUsersFetcher = new DocumentsUsersFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsUsersFetcher);

            _documentsCategoriesFetcher = new DocumentsCategoriesFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsCategoriesFetcher);

            _documentsInternalCategoriesFetcher = new DocumentsInternalCategoriesFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsInternalCategoriesFetcher);
        }
    }
}
