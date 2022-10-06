using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class DynamicFormRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> DynamicFormUsers
            => GetItems<DynamicFormsUsersFetcher, UserModel>();

        public IReadOnlyList<ArchivedDocumentCategoryModel> Categories
            => GetItems<GenericArchivedDocumentsCategoriesFetcher, ArchivedDocumentCategoryModel>();

        public IReadOnlyList<DepartmentModel> Departments
            => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public IReadOnlyList<DynamicFormFieldMapping> DynamicFormFieldMappings
            => GetItems<DynamicFormFieldValuesFetcher, DynamicFormFieldMapping>();

        public DynamicFormRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericArchivedDocumentsCategoriesFetcher>();
        }

        public DynamicFormRelationsFetcher UseDynamicFormsContext(DynamicFormsFetcherContext context)
        {
            Aggregator
               .UseRemoteFetcher<DynamicFormsUsersFetcher>(context)
               .UseRemoteFetcher<GenericDepartmentsFetcher>(context)
               .UseInternalFetcher<DynamicFormFieldValuesFetcher>(context);
               
            return this;
        }
    }
}
