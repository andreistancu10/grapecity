using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class PublicAcquisitionProjectFetcher : ModelFetcher<PublicAcquisitionProject, ModelFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        public PublicAcquisitionProjectFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
        }

        protected override Task<List<PublicAcquisitionProject>> FetchInternalAsync(ModelFetcherContext context, CancellationToken cancellationToken)
        {
            return _dbContext.PublicAcquisitionProjects
                             .ToListAsync(cancellationToken);
        }
    }
}
