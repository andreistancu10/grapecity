using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class GeneralObjectiveRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> GeneralObjectiveUsers
            => GetItems<GeneralObjectivesUsersFetcher, UserModel>();
        public GeneralObjectiveRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        { }

        public GeneralObjectiveRelationsFetcher UseGeneralObjectivesContext(GeneralObjectivesFetcherContext context) {
            Aggregator
               .UseRemoteFetcher<GeneralObjectivesUsersFetcher>(context);

            return this;
        }
    }
}
