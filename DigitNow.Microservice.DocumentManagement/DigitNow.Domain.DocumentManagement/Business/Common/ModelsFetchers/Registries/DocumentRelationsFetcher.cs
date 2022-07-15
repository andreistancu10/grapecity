using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentRelationsFetcher
    {
        private readonly IModelFetcher<UserModel, DocumentsFetcherContext> _documentsUsersFetcher;
        private readonly IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsCategoriesFetcher;
        private readonly IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsInternalCategoriesFetcher;
        private readonly IModelFetcher<DocumentDepartmentModel, DocumentsFetcherContext> _documentsDepartmentsFetcher;
        private readonly IModelFetcher<DocumentsSpecialRegisterMappingModel, DocumentsFetcherContext> _documentsSpecialRegisterMappingFetcher;

        public IReadOnlyList<UserModel> DocumentUsers { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories { get; private set; }
        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories { get; private set; }
        public IReadOnlyList<DocumentDepartmentModel> DocumentDepartments { get; set; }
        public IReadOnlyList<DocumentsSpecialRegisterMappingModel> DocumentSpecialRegisterMapping { get; set; }

        public DocumentRelationsFetcher(IServiceProvider serviceProvider)
        {
            _documentsUsersFetcher = new DocumentsUsersFetcher(serviceProvider);
            _documentsCategoriesFetcher = new DocumentsCategoriesFetcher(serviceProvider);
            _documentsInternalCategoriesFetcher = new DocumentsInternalCategoriesFetcher(serviceProvider);
            _documentsDepartmentsFetcher = new DocumentsDepartmentsFetcher(serviceProvider);
            _documentsSpecialRegisterMappingFetcher = new DocumentsSpecialRegisterMappingFetcher(serviceProvider);
        }

        public async Task FetchRelationshipsAsync(DocumentsFetcherContext context, CancellationToken cancellationToken)
        {
            var documentUsersTask = _documentsUsersFetcher.FetchAsync(context, cancellationToken);
            var documentCategoriesTask = _documentsCategoriesFetcher.FetchAsync(context, cancellationToken);
            var documentInternalCategoriesTask = _documentsInternalCategoriesFetcher.FetchAsync(context, cancellationToken);
            var documentDepartmentsTask = _documentsDepartmentsFetcher.FetchAsync(context, cancellationToken);
            var documentSpecialRegisterMappingTask = _documentsSpecialRegisterMappingFetcher.FetchAsync(context, cancellationToken);

            await Task.WhenAll(documentUsersTask,
                documentCategoriesTask,
                documentInternalCategoriesTask,
                documentDepartmentsTask,
                documentSpecialRegisterMappingTask);

            DocumentUsers = await documentUsersTask;
            DocumentCategories = await documentCategoriesTask;
            DocumentInternalCategories = await documentInternalCategoriesTask;
            DocumentDepartments = await documentDepartmentsTask;
            DocumentSpecialRegisterMapping = await documentSpecialRegisterMappingTask;
        }
    }
}
