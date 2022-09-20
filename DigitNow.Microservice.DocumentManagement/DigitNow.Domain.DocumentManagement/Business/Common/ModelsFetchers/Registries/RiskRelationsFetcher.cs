using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class RiskRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();
        public IReadOnlyList<ObjectiveModel> SpecificObjective => GetItems<RiskSpecificObjectiveFetcher, ObjectiveModel>();
        public RiskRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider){}

        public RiskRelationsFetcher UseRiskFetcherContext(RisksFetcherContext context)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseInternalFetcher<RiskSpecificObjectiveFetcher>(context);

            return this;
        }
    }
}
