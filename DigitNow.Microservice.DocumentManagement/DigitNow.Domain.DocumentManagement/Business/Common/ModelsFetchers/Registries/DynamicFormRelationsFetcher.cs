﻿using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class DynamicFormRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> DynamicFormUsers
            => GetItems<DynamicFormsUsersFetcher, UserModel>();

        public IReadOnlyList<DocumentDepartmentModel> Departments
            => GetItems<GenericDocumentsDepartmentsFetcher, DocumentDepartmentModel>();

        public DynamicFormRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public DynamicFormRelationsFetcher UseDynamicFormsContext(DynamicFormsFetcherContext context)
        {
            Aggregator
               .UseRemoteFetcher<DynamicFormsUsersFetcher>(context)
               .UseRemoteFetcher<GenericDocumentsDepartmentsFetcher>(context);
               
            return this;
        }
    }
}
