using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts
{
    internal class FormFieldFetcherContext : ModelFetcherContext
    {
        public IList<FormFieldMapping> FormFieldMappings
        {
            get => this[nameof(FormFieldMappings)] as IList<FormFieldMapping>;
            set => this[nameof(FormFieldMappings)] = value;
        }
    }
}