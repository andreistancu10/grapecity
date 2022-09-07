using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class ActivityRelationsFetcher : BaseRelationsFetcher
    {
        public IReadOnlyList<UserModel> Users => GetItems<ActivityUsersFetcher, UserModel>();
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();
        public IReadOnlyList<ObjectiveModel> SpecificObjective => GetItems<ActivitySpecificObjectiveFetcher, ObjectiveModel>();
        public IReadOnlyList<ObjectiveModel> GeneralObjective => GetItems<ActivityGeneralObjectiveFetcher, ObjectiveModel>();

        public ActivityRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public ActivityRelationsFetcher UseActivityFetcherContext(ActivitiesFetcherContext context)
        {
            Aggregator
                .UseRemoteFetcher<ActivityUsersFetcher>(context)
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseInternalFetcher<ActivitySpecificObjectiveFetcher>(context)
                .UseInternalFetcher<ActivityGeneralObjectiveFetcher>(context);

            return this;
        }
    }
}