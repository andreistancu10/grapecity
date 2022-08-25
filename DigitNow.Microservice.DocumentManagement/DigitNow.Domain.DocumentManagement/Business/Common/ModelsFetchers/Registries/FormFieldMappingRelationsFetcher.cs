using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.Registries
{
    internal sealed class FormFieldMappingRelationsFetcher : BaseRelationsFetcher
    {
        public List<DynamicFormFieldModel> FormFieldModels
            => GetItems<FormFieldFetcher, DynamicFormFieldModel>();

        public FormFieldMappingRelationsFetcher(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public FormFieldMappingRelationsFetcher UseFormFieldContext(FormFieldFetcherContext context)
        {
            Aggregator.UseInternalFetcher<FormFieldFetcher>(context);

            return this;
        }
    }
}