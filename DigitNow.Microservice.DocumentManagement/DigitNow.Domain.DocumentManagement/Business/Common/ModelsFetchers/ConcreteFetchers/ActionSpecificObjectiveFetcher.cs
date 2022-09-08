using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchersContexts;
using DigitNow.Domain.DocumentManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitNow.Domain.DocumentManagement.Business.Common.ModelsFetchers.ConcreteFetchers
{
    internal class ActionSpecificObjectiveFetcher : ModelFetcher<ObjectiveModel, ActionsFetcherContext>
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActionSpecificObjectiveFetcher(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<DocumentManagementDbContext>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        protected override async Task<List<ObjectiveModel>> FetchInternalAsync(ActionsFetcherContext context, CancellationToken cancellationToken)
        {
            var activityIds = context.Actions.Select(c => c.ActivityId);
            var result = await _dbContext.Activities
                .Where(c => activityIds.Contains(c.Id))
                .Include(c => c.AssociatedSpecificObjective)
                .ThenInclude(c => c.Objective)
                .Select(c => c.AssociatedSpecificObjective)
                .ToListAsync(cancellationToken);

            return result.Select(c => _mapper.Map<ObjectiveModel>(c)).ToList();
        }
    }
}