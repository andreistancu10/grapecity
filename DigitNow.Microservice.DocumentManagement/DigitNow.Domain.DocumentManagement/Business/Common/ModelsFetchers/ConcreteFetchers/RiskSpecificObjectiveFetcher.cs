using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class RiskSpecificObjectiveFetcher : ModelFetcher<ObjectiveModel, RisksFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public RiskSpecificObjectiveFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<ObjectiveModel>> FetchInternalAsync(RisksFetcherContext context, CancellationToken cancellationToken)
        {
            var result = await _dbContext.SpecificObjectives
                .Include(c => c.Objective)
                .ToListAsync(cancellationToken);

            return result.Select(c => _mapper.Map<ObjectiveModel>(c)).ToList();
        }
    }
}
