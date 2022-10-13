using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class SpecificObjectiveRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> ObjectiveUsers
            => GetItems<ObjectiveUserFetcher, UserModel>();

        public IReadOnlyList<DepartmentModel> ObjectiveDepartments
            => GetItems<GenericDepartmentsFetcher, DepartmentModel>();

        public IReadOnlyList<StateModel> States 
            => GetItems<GenericStatesFetcher, StateModel>();

        public SpecificObjectiveRelationsFetcher(IServiceProvider serviceProvider)
           : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseGenericRemoteFetcher<GenericStatesFetcher>();
        }

        public SpecificObjectiveRelationsFetcher UseSpecificObjectiveContext(ObjectivesFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<ObjectiveUserFetcher>(context);

            return this;
        }
    }
}
