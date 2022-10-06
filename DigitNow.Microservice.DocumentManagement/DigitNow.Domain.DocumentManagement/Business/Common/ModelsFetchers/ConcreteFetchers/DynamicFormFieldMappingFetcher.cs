using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class DynamicFormFieldMappingFetcher : ModelFetcher<DynamicFormFieldMapping, ModelFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DynamicFormFieldMappingFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        protected override Task<List<DynamicFormFieldMapping>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            return  _dbContext.DynamicFormFieldMappings
                .Include(c => c.DynamicFormFieldValues)
                .ToListAsync(cancellationToken);
        }
    }
}
