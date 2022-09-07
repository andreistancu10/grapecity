using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActivityGeneralObjectiveFetcher : ModelFetcher<ObjectiveModel, ActivitiesFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActivityGeneralObjectiveFetcher(
            DocumentManagementDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected override async Task<List<ObjectiveModel>> FetchInternalAsync(ActivitiesFetcherContext context, CancellationToken cancellationToken)
        {
            var generalObjectivesId = context.Activities.Select(c => c.GeneralObjectiveId);
            var result = await _dbContext.GeneralObjectives
                .Where(c => generalObjectivesId.Contains(c.Id))
                .Include(c => c.Objective)
                .Select(c => c.Objective)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ObjectiveModel>>(result);
        }
    }
}