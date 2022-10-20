using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IPerformanceIndicatorService : IScimStateService
    {
        Task<PerformanceIndicator> AddAsync(PerformanceIndicator performanceIndicator, CancellationToken cancellationToken);
        Task UpdateAsync(PerformanceIndicator performanceIndicator, CancellationToken cancellationToken);
        IQueryable<PerformanceIndicator> GetByIdQuery(long performanceIndicatorId);
    }
    public class PerformanceIndicatorService : ScimStateService, IPerformanceIndicatorService
    {
        private readonly ICatalogClient _catalogClient;

        public PerformanceIndicatorService(DocumentManagementDbContext dbContext, ICatalogClient catalogClient) : base(dbContext)
        {
            _catalogClient = catalogClient;
        }

        public async Task<PerformanceIndicator> AddAsync(PerformanceIndicator performanceIndicator, CancellationToken cancellationToken)
        {
            var activeScimState = await _catalogClient.ScimStates.GetScimStateByCodeAsync("activ", cancellationToken);
            performanceIndicator.StateId = activeScimState.Id;

            var dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                DbContext.Entry(performanceIndicator).State = EntityState.Added;
                await SetPerformanceIndicatorCode(performanceIndicator, cancellationToken);
               
                await DbContext.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return performanceIndicator;
        }

        private async Task SetPerformanceIndicatorCode(PerformanceIndicator performanceIndicator, CancellationToken cancellationToken)
        {
            var lastPerformanceIndicatorId = await DbContext.PerformanceIndicators
                .Where(item => item.CreatedAt.Year == DateTime.UtcNow.Year)
                .OrderByDescending(p => p.CreatedAt)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            performanceIndicator.Code = $"INDICPERF{DateTime.Now.Year}_{++lastPerformanceIndicatorId}";
        }

        public IQueryable<PerformanceIndicator> GetByIdQuery(long performanceIndicatorId)
        {
            return DbContext.PerformanceIndicators.Where(x => x.Id == performanceIndicatorId).AsQueryable();
        }

        public async Task UpdateAsync(PerformanceIndicator performanceIndicator, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { performanceIndicator.Id }, ScimEntity.ScimPerformanceIndicator, performanceIndicator.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(performanceIndicator, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
