using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class FormFieldFetcher : ModelFetcher<DynamicFormFieldModel, FormFieldFetcherContext>
    {
        protected override Task<List<DynamicFormFieldModel>> FetchInternalAsync(FormFieldFetcherContext context, CancellationToken cancellationToken)
        {
            var formFieldModels = context.FormFieldMappings.Select(c => new DynamicFormFieldModel
            {
                Id = c.FormField.Id,
                Name = c.FormField.Name,
                Context = c.FormField.Context,
                DynamicFieldType = c.FormField.DynamicFieldType
            }).ToList();

            return Task.FromResult(formFieldModels);
        }
    }
}