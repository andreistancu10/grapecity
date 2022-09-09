using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActivityGeneralObjectiveFetcher : ModelFetcher<ObjectiveModel, ActivitiesFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActivityGeneralObjectiveFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<ObjectiveModel>> FetchInternalAsync(ActivitiesFetcherContext context, CancellationToken cancellationToken)
        {
            var generalObjectivesId = context.Activities.Select(c => c.GeneralObjectiveId);
            var result = await _dbContext.GeneralObjectives
                .Where(c => generalObjectivesId.Contains(c.ObjectiveId))
                .Include(c => c.Objective)
                .ToListAsync(cancellationToken);

            return result.Select(c => _mapper.Map<ObjectiveModel>(c)).ToList();
        }
    }
}