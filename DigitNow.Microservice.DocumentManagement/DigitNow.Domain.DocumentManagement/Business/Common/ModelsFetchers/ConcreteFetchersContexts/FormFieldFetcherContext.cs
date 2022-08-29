using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class FormFieldFetcherContext : ModelFetcherContext
    {
        public IList<DynamicFormFieldMapping> FormFieldMappings
        {
            get => this[nameof(FormFieldMappings)] as IList<DynamicFormFieldMapping>;
            set => this[nameof(FormFieldMappings)] = value;
        }
    }
}