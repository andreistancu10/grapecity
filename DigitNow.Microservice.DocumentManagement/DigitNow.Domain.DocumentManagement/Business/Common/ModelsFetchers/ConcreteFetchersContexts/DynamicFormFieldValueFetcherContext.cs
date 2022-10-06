using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class DynamicFormFieldValueFetcherContext : ModelFetcherContext
    {
        public IList<DynamicFormFieldValue> DynamicFormFieldValues
        {
            get => this[nameof(DynamicFormFieldValues)] as IList<DynamicFormFieldValue>;
            set => this[nameof(DynamicFormFieldValues)] = value;
        }
    }
}
