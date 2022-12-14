using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class ActionRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> Users => GetItems<ActionUsersFetcher, UserModel>();
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();
        public IReadOnlyList<StateModel> States => GetItems<GenericStatesFetcher, StateModel>();
        public IReadOnlyList<ObjectiveModel> SpecificObjective => GetItems<ActionSpecificObjectiveFetcher, ObjectiveModel>();
        public IReadOnlyList<ObjectiveModel> GeneralObjective => GetItems<ActionGeneralObjectiveFetcher, ObjectiveModel>();

        public ActionRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseGenericRemoteFetcher<GenericStatesFetcher>();
        }

        public ActionRelationsFetcher UseActionFetcherContext(ActionsFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<ActionUsersFetcher>(context)
                .UseInternalFetcher<ActionSpecificObjectiveFetcher>(context)
                .UseInternalFetcher<ActionGeneralObjectiveFetcher>(context);

            return this;
        }
    }
}