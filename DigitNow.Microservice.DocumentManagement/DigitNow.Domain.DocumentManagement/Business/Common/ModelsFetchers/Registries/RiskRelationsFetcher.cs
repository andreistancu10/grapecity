using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class RiskRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<DepartmentModel> Departments 
            => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public IReadOnlyList<StateModel> States 
            => GetItems<GenericStatesFetcher, StateModel>();

        public IReadOnlyList<ObjectiveModel> SpecificObjective 
            => GetItems<RiskSpecificObjectiveFetcher, ObjectiveModel>();

        public RiskRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider) 
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseGenericRemoteFetcher<GenericStatesFetcher>();
        }

        public RiskRelationsFetcher UseRiskFetcherContext(RisksFetcherContext context)
        {
            Aggregator                
                .UseInternalFetcher<RiskSpecificObjectiveFetcher>(context);

            return this;
        }
    }
}
