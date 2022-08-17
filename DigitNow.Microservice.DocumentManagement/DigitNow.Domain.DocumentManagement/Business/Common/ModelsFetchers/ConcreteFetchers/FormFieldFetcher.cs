using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class FormFieldFetcher : ModelFetcher<FormFieldModel, FormFieldFetcherContext>
    {
        protected override Task<List<FormFieldModel>> FetchInternalAsync(FormFieldFetcherContext context, CancellationToken cancellationToken)
        {
            var formFieldModels = context.FormFieldMappings.Select(c => new FormFieldModel
            {
                Id = c.FormField.Id,
                Name = c.FormField.Name,
                Context = c.FormField.Context,
                FieldType = c.FormField.FieldType
            }).ToList();

            return Task.FromResult(formFieldModels);
        }
    }
}