using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActivitySpecificObjectiveFetcher : ModelFetcher<ObjectiveModel, ActivitiesFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActivitySpecificObjectiveFetcher(
            DocumentManagementDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected override async Task<List<ObjectiveModel>> FetchInternalAsync(ActivitiesFetcherContext context, CancellationToken cancellationToken)
        {
            var specificObjectiveIds = context.Activities.Select(c => c.SpecificObjectiveId);
            var result = await _dbContext.SpecificObjectives
                .Where(c => specificObjectiveIds.Contains(c.Id))
                .Include(c => c.Objective)
                .Select(c => c.Objective)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ObjectiveModel>>(result);
        }
    }
}