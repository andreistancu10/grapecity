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
                Id = c.DynamicFormField.Id,
                Name = c.DynamicFormField.Name,
                Context = c.DynamicFormField.Context,
                DynamicFieldType = c.DynamicFormField.DynamicFieldType
            }).ToList();

            return Task.FromResult(formFieldModels);
        }
    }
}