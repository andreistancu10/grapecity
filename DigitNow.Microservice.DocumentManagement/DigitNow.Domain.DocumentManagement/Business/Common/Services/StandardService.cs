using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Standards;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Standards;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IStandardService
    {
        Task<Standard> CreateAsync(Standard standard, CancellationToken cancellationToken);
        Task UpdateAsync(Standard standard, CancellationToken cancellationToken);
        IQueryable<Standard> FindQuery();
        Task<long> CountAsync(StandardFilter filter, CancellationToken cancellationToken);
        Task<List<Standard>> GetAllAsync(StandardFilter filter, int page, int count, CancellationToken cancellationToken);
    }
    public class StandardService : ScimStateService, IStandardService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogClient _catalogClient;
        private readonly IIdentityService _identityService;
        private readonly DocumentManagementDbContext _dbContext;


        public StandardService(
            DocumentManagementDbContext dbContext,
            ICatalogClient catalogClient,
            IServiceProvider serviceProvider,
            IIdentityService identityService) : base(dbContext)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
            _identityService = identityService;
        }
        public async Task<Standard> CreateAsync(Standard standard, CancellationToken cancellationToken)
        {
            var activeScimState = await _catalogClient.ScimStates.GetScimStateByCodeAsync("activ", cancellationToken);
            standard.StateId = activeScimState.Id;

            await SetStandardCodeAsync(standard, cancellationToken);
            await DbContext.Standards.AddAsync(standard, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);

            return standard;
        }
        private async Task SetStandardCodeAsync(Standard standard, CancellationToken token)
        {
            var lastStandardId = await DbContext.Standards
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.CreatedAt)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(token);

            standard.Code = $"STANDARD{DateTime.Now.Year}_{++lastStandardId}";
           
        }

        public IQueryable<Standard> FindQuery()
        {
            return DbContext.Standards.AsQueryable();
        }

        public async Task UpdateAsync(Standard standard, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { standard.Id }, ScimEntity.ScimAction, standard.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(standard, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<long> CountAsync(StandardFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.Standards
                .WhereAll((await GetStandardsExpressions(filter, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }
        private async Task<DataExpressions<Standard>> GetStandardsExpressions(StandardFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Standard>();

            dataExpressions.AddRange(await GetStandardExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetStandardUserRightsExpressionsAsync(token));

            return dataExpressions;
        }
        private async Task<DataExpressions<Standard>> GetStandardUserRightsExpressionsAsync(CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var rightsComponent = new StandardsPermissionsFilterComponent(_serviceProvider);
            var rightsComponentContext = new StandardsPermissionsFilterComponentContext
            {
                CurrentUser = currentUser
            };

            return await rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
        }
        private Task<DataExpressions<Standard>> GetStandardExpressionsAsync(StandardFilter filter, CancellationToken token)
        {
            var filterComponent = new StandardsFilterComponent(_serviceProvider);
            var filterComponentContext = new StandardsFilterComponentContext
            {
                StandardFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        public async Task<List<Standard>> GetAllAsync(StandardFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            return await _dbContext.Standards
                 .WhereAll((await GetStandardsExpressions(filter, cancellationToken)).ToPredicates())
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);
        }
    }
}
