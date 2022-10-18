using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class StandardRelationsFetcher : BaseRelationsFetcher
    {
        public StandardRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public StandardRelationsFetcher UseStandardFetcherContext()
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>();

            return this;
        }
    }
}
