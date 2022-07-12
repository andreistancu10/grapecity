using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers
{
    internal sealed class DocumentRelationsFetcher
    {
        private IModelFetcher<UserModel, DocumentsFetcherContext> _documentsUsersFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsCategoriesFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsInternalCategoriesFetcher;

        public IReadOnlyList<UserModel> DocumentUsers { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories { get; private set; }

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
        {
            _documentsUsersFetcher = new DocumentsUsersFetcher(serviceProvider);
            _documentsCategoriesFetcher = new DocumentsCategoriesFetcher(serviceProvider);
            _documentsInternalCategoriesFetcher = new DocumentsInternalCategoriesFetcher(serviceProvider);
        }

        public async Task FetchRelationshipsAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var documentUsersTask = _documentsUsersFetcher.FetchAsync(context, cancellationToken);
            var documentCategoriesTask = _documentsCategoriesFetcher.FetchAsync(context, cancellationToken);
            var documentInternalCategoriesTask = _documentsInternalCategoriesFetcher.FetchAsync(context, cancellationToken);

            await Task.WhenAll(documentUsersTask, documentCategoriesTask, documentInternalCategoriesTask);

            DocumentUsers = await documentUsersTask;
            DocumentCategories = await documentCategoriesTask;
            DocumentInternalCategories = await documentInternalCategoriesTask;
        }
    }
}
