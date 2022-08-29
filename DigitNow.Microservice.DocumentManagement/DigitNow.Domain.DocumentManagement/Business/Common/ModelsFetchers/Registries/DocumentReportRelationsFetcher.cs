﻿using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class DocumentReportRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> DocumentUsers 
            => GetItems<DocumentsUsersFetcher, UserModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentCategories 
            => GetItems<GenericDocumentsCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<DocumentCategoryModel> DocumentInternalCategories 
            => GetItems<GenericDocumentsInternalCategoriesFetcher, DocumentCategoryModel>();

        public IReadOnlyList<DocumentDepartmentModel> DocumentDepartments 
            => GetItems<GenericDocumentsDepartmentsFetcher, DocumentDepartmentModel>();

        public IReadOnlyList<DocumentStatusTranslationModel> DocumentStatusTranslations
            => GetItems<DocumentStatusTranslationsFetcher, DocumentStatusTranslationModel>();

        public IReadOnlyList<DocumentTypeTranslationModel> DocumentTypesTranslationModels
            => GetItems<DocumentTypeTranslationsFetcher, DocumentTypeTranslationModel>();

        public DocumentReportRelationsFetcher(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDocumentsCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDocumentsInternalCategoriesFetcher>()
                .UseGenericRemoteFetcher<GenericDocumentsDepartmentsFetcher>();
        }

        public DocumentReportRelationsFetcher UseDocumentsContext(DocumentsFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<DocumentsUsersFetcher>(context);

            return this;
        }

        public DocumentReportRelationsFetcher UseTranslationsContext(LanguageFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<DocumentStatusTranslationsFetcher>(context)
                .UseRemoteFetcher<DocumentTypeTranslationsFetcher>(context);

            return this;
        }
    }
}
