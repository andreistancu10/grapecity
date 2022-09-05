using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class DynamicFormsFetcherContext : ModelFetcherContext
    {
        public IList<DynamicFormFillingLog> DynamicFormFillingLogs
        {
            get => this[nameof(DynamicFormFillingLog)] as IList<DynamicFormFillingLog>;
            set => this[nameof(DynamicFormFillingLog)] = value;
        }
    }
}
