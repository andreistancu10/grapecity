using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Objectives;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IObjectiveDashboardService
    {
        Task<long> CountGeneralObjectivesAsync(ObjectiveFilter filter, CancellationToken token);
        Task<List<GeneralObjective>> GetGeneralObjectivesAsync(ObjectiveFilter filter, int page, int count, CancellationToken token);
    }

    public class ObjectiveDashboardService : IObjectiveDashboardService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public ObjectiveDashboardService( 
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<long> CountGeneralObjectivesAsync(ObjectiveFilter filter, CancellationToken token)
        {
            return await _dbContext.GeneralObjectives
                .WhereAll((await GetObjectivesExpressionsAsync(filter, token)).ToPredicates())
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<List<GeneralObjective>> GetGeneralObjectivesAsync(ObjectiveFilter filter, int page, int count, CancellationToken token)
        {
            var objectives = await _dbContext.GeneralObjectives
                .Include(x => x.Objective)
                 .WhereAll((await GetObjectivesExpressionsAsync(filter, token)).ToPredicates())
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .AsNoTracking()
                 .ToListAsync(token);
            return objectives;
        }

        private Task<DataExpressions<GeneralObjective>> GetObjectivesExpressionsAsync(ObjectiveFilter filter, CancellationToken token)
        {
            var filterComponent = new ObjectivesFilterComponent(_serviceProvider);
            var filterComponentContext = new ObjectivesFilterComponentContext
            {
                ObjectiveFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }
    }
}
