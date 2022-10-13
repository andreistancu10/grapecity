using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class GeneralObjectiveRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> GeneralObjectiveUsers
            => GetItems<GeneralObjectivesUsersFetcher, UserModel>();

        public IReadOnlyList<StateModel> States 
            => GetItems<GenericStatesFetcher, StateModel>();

        public GeneralObjectiveRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericStatesFetcher>();
        }

        public GeneralObjectiveRelationsFetcher UseGeneralObjectivesContext(GeneralObjectivesFetcherContext context) {
            Aggregator
               .UseRemoteFetcher<GeneralObjectivesUsersFetcher>(context);

            return this;
        }
    }
}
