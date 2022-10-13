using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IPublicAcquisitionService
    {
        // Create
        Task<PublicAcquisitionProject> AddAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken);

        // Read
        IQueryable<PublicAcquisitionProject> GetByIdQuery(long publicAcquisitionId);

        // Update
        Task UpdateAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken);

    }
    public class PublicAcquisitionService : IPublicAcquisitionService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public PublicAcquisitionService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<PublicAcquisitionProject> GetByIdQuery(long publicAcquisitionId)
        {
            return _dbContext.PublicAcquisitionProjects.Where(pa => pa.Id == publicAcquisitionId);
        }

        public async Task<PublicAcquisitionProject> AddAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            try
            {
                _dbContext.Entry(publicAcquisition).State = EntityState.Added;
                await SetPublicAcquisitionCodeAsync(publicAcquisition, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

            return publicAcquisition;
        }

        private async Task SetPublicAcquisitionCodeAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            var lastPublicAquisitionCode = await _dbContext.PublicAcquisitionProjects
                .OrderByDescending(p => p.CreatedAt)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            publicAcquisition.Code = $"{++lastPublicAquisitionCode}";
        }

        public async Task UpdateAsync(PublicAcquisitionProject publicAcquisition, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(publicAcquisition, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
