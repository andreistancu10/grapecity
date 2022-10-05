using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.GenericFetchers;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal class ProcedureRelationsFetcher: BaseRelationsFetcher
    {
        public ProcedureRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public IReadOnlyList<DepartmentModel> Departments => GetItems<GenericDepartmentsFetcher, DepartmentModel>();
        public IReadOnlyList<ObjectiveModel> SpecificObjectives => GetItems<ProcedureSpecificObjectivesFetcher, ObjectiveModel>();
        public IReadOnlyList<UserModel> Users => GetItems<ProceduresUsersFetcher, UserModel>();

        public ProcedureRelationsFetcher UseProcedureFetcherContext(ProceduresFetcherContext context)
        {
            Aggregator
                .UseGenericRemoteFetcher<GenericDepartmentsFetcher>()
                .UseInternalFetcher<ProcedureSpecificObjectivesFetcher>(context)
                .UseInternalFetcher<ProceduresUsersFetcher>(context);

            return this;
        }
    }
}
