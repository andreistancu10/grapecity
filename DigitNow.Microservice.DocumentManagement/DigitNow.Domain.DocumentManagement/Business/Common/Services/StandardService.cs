using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IStandardService
    {
        Task<Standard> CreateAsync(Standard standard, CancellationToken cancellationToken);
        Task UpdateAsync(Standard standard, CancellationToken cancellationToken);
        IQueryable<Standard> FindQuery();
    }
    public class StandardService : ScimStateService, IStandardService
    {
        private readonly ICatalogClient _catalogClient;

        public StandardService(
            DocumentManagementDbContext dbContext,
            ICatalogClient catalogClient) : base(dbContext)
        {
            _catalogClient = catalogClient; 
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
            await ChangeStateAsync(new List<long> { standard.Id }, ScimEntity.ScimStandard, standard.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(standard, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
