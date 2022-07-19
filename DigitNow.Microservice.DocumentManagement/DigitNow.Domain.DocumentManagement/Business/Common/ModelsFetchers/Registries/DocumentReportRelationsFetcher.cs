using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentReportRelationsFetcher : RelationalAggregateFetcher<DocumentsFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IModelFetcher<UserModel, DocumentsFetcherContext> _documentsUsersFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsCategoriesFetcher;
        private IModelFetcher<DocumentCategoryModel, DocumentsFetcherContext> _documentsInternalCategoriesFetcher;
        private IModelFetcher<DocumentDepartmentModel, DocumentsFetcherContext> _documentsDepartmentsFetcher;
        private IModelFetcher<DocumentsSpecialRegisterMappingModel, DocumentsFetcherContext> _documentsSpecialRegisterMappingFetcher;

        public IReadOnlyList<UserModel> DocumentUsers => GetItems(_documentsUsersFetcher);
        public IReadOnlyList<DocumentCategoryModel> DocumentCategories => GetItems(_documentsCategoriesFetcher);
        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories => GetItems(_documentsInternalCategoriesFetcher);
        public IReadOnlyList<DocumentDepartmentModel> DocumentDepartments => GetItems(_documentsDepartmentsFetcher);
        public IReadOnlyList<DocumentsSpecialRegisterMappingModel> DocumentSpecialRegisterMapping => GetItems(_documentsSpecialRegisterMappingFetcher);

        public DocumentReportRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void AddInternalFetchers()
        {
            _documentsSpecialRegisterMappingFetcher = new DocumentsSpecialRegisterMappingFetcher(_serviceProvider);
            InternalFetchers.Add(_documentsSpecialRegisterMappingFetcher);
        }

        protected override void AddRemoteFetchers()
        {
            _documentsUsersFetcher = new DocumentsUsersFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsUsersFetcher);

            _documentsCategoriesFetcher = new DocumentsCategoriesFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsCategoriesFetcher);

            _documentsInternalCategoriesFetcher = new DocumentsInternalCategoriesFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsInternalCategoriesFetcher);

            _documentsDepartmentsFetcher = new DocumentsDepartmentsFetcher(_serviceProvider);
            RemoteFetchers.Add(_documentsDepartmentsFetcher);
        }
    }
}
