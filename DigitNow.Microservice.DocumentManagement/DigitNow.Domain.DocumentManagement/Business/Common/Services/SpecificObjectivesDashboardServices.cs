using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Objectives;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Objectives;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.SpecificObjectives;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecificObjectivesDashboardServices
    {
        Task<long> CountSpecificObjectivesAsync(SpecificObjectiveFilter filter, CancellationToken token);
        Task<List<SpecificObjective>> GetSpecificObjectivesAsync(SpecificObjectiveFilter filter, int page, int count, CancellationToken token);
    }
    public class SpecificObjectivesDashboardServices : ISpecificObjectivesDashboardServices
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly IIdentityService _identityService;

        public SpecificObjectivesDashboardServices(DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _identityService = identityService;
        }

        public async Task<long> CountSpecificObjectivesAsync(SpecificObjectiveFilter filter, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            return await GetBuiltInSpecificObjectivesQuery()
                .WhereAll((await GetSpecificObjectivesExpressions(currentUser, filter, token)).ToPredicates())
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<List<SpecificObjective>> GetSpecificObjectivesAsync(SpecificObjectiveFilter filter, int page, int count, CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var specificObjectives = await GetBuiltInSpecificObjectivesQuery()
                 .WhereAll((await GetSpecificObjectivesExpressions(currentUser, filter, token)).ToPredicates())
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .AsNoTracking()
                 .ToListAsync(token);

            return specificObjectives;
        }

        private async Task<DataExpressions<SpecificObjective>> GetSpecificObjectivesExpressions(UserModel currentUser, SpecificObjectiveFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<SpecificObjective>();

            dataExpressions.AddRange(await GetSpecificObjectivesExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetSpecificObjectivesUserRightsExpressionsAsync(currentUser, token));

            return dataExpressions;
        }

        private Task<DataExpressions<SpecificObjective>> GetSpecificObjectivesExpressionsAsync(SpecificObjectiveFilter filter, CancellationToken token)
        {
            var filterComponent = new SpecificObjectivesFilterComponenet(_serviceProvider);
            var filterComponentContext = new SpecificObjectivesFilterComponenetContext
            {
                SpecificObjectiveFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        private Task<DataExpressions<SpecificObjective>> GetSpecificObjectivesUserRightsExpressionsAsync(UserModel currentUser, CancellationToken token)
        {
            var rightsComponent = new SpecificObjectivesPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new SpecificObjectivesPermissionsFilterComponentContext
            {
                CurrentUser = currentUser
            };

            return rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }

        private IQueryable<SpecificObjective> GetBuiltInSpecificObjectivesQuery()
        {
            return _dbContext.SpecificObjectives
                 .Include(x => x.Objective)
                 .Include(x => x.AssociatedGeneralObjective)
                 .Include(x => x.AssociatedGeneralObjective);
        }
    }
}
