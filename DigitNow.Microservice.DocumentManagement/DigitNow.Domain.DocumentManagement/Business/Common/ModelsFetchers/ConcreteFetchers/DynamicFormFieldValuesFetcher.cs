using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DynamicFormFieldValuesFetcher : ModelFetcher<DynamicFormFieldMapping, ModelFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DynamicFormFieldValuesFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        protected override async Task<List<DynamicFormFieldMapping>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            return await _dbContext.DynamicFormFieldMappings
                .Include(c => c.DynamicFormFieldValues)
                .ToListAsync(cancellationToken);
        }
    }
}
