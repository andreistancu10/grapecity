﻿using System;
using System.Collections.Generic;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class UploadedFileRelationsFetcher : RelationalAggregateFetcher<UploadedFilesFetcherContext>
    {
        private readonly IServiceProvider _serviceProvider;

        private IModelFetcher<DocumentCategoryModel, UploadedFilesFetcherContext> _uploadedFilesCategoriesFetcher;
        private IModelFetcher<UserModel, UploadedFilesFetcherContext> _uploadedFilesUsersFetcher;

        public IReadOnlyList<DocumentCategoryModel> UploadedFileCategoryModels => GetItems(_uploadedFilesCategoriesFetcher);
        public IReadOnlyList<UserModel> UploadedFileUsers => GetItems(_uploadedFilesUsersFetcher);

        public UploadedFileRelationsFetcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void AddRemoteFetchers()
        {
            _uploadedFilesCategoriesFetcher = new UploadedFilesDocumentCategoriesFetcher(_serviceProvider);
            RemoteFetchers.Add(_uploadedFilesCategoriesFetcher);
            
            _uploadedFilesUsersFetcher = new UploadedFilesUsersFetcher(_serviceProvider);
            RemoteFetchers.Add(_uploadedFilesUsersFetcher);
        }
    }
}