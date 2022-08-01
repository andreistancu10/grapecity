using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Documents.Postprocess;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Documents.Postprocess
{
    internal class ActiveDocumentsPostprocessFilterComponent<T> : DataExpressionFilterComponent<T, ActiveDocumentsPostprocessFilterComponentContext>
        where T : VirtualDocument
    {
        public ActiveDocumentsPostprocessFilterComponent(IServiceProvider serviceProvider) 
            : base(serviceProvider) { }

        protected override Task<DataExpressions<T>> SetCustomDataExpressionsAsync(ActiveDocumentsPostprocessFilterComponentContext context, CancellationToken token)            
        {
            if (!context.PostprocessFilter.IsEmpty()) 
                return Task.FromResult(new DataExpressions<T>());

            return Task.FromResult(new DocumentPostprocessFilterBuilder<T>(ServiceProvider, context.PostprocessFilter)
                .Build());
        }
    }
}
