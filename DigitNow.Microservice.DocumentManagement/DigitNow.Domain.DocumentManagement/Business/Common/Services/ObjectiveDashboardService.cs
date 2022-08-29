using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.GeneralObjectives;
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
        Task<long> CountGeneralObjectivesAsync(GeneralObjectiveFilter filter, CancellationToken token);
        Task<List<GeneralObjective>> GetGeneralObjectivesAsync(GeneralObjectiveFilter filter, int page, int count, CancellationToken token);
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

        public async Task<long> CountGeneralObjectivesAsync(GeneralObjectiveFilter filter, CancellationToken token)
        {
            return await _dbContext.GeneralObjectives
                .WhereAll((await GetObjectivesExpressionsAsync(filter, token)).ToPredicates())
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<List<GeneralObjective>> GetGeneralObjectivesAsync(GeneralObjectiveFilter filter, int page, int count, CancellationToken token)
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

        private Task<DataExpressions<GeneralObjective>> GetObjectivesExpressionsAsync(GeneralObjectiveFilter filter, CancellationToken token)
        {
            var filterComponent = new GeneralObjectivesFilterComponent(_serviceProvider);
            var filterComponentContext = new GeneralObjectivesFilterComponentContext
            {
                GeneralObjectiveFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }
    }
}
