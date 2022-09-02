using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.DynamicForms;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.DynamicForms
{
    internal class DynamicFormsFilterComponent : DataExpressionFilterComponent<DynamicFormFillingLog, DynamicFormsFilterComponentContext>
    {
        public DynamicFormsFilterComponent(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override Task<DataExpressions<DynamicFormFillingLog>> SetCustomDataExpressionsAsync(DynamicFormsFilterComponentContext context, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<DynamicFormFillingLog>();

            if (!context.DynamicFormFilter.IsEmpty())
            {
                var filterBuilder = new DynamicFormsFilterBuilder(ServiceProvider, context.DynamicFormFilter);
                dataExpressions.AddRange(filterBuilder.Build());
            }

            return Task.FromResult(dataExpressions);
        }
    }
}
