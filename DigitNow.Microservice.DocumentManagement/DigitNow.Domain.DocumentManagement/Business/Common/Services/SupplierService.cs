using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Suppliers;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Suppliers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{

    public interface ISupplierService
    {
        Task<Supplier> AddAsync(Supplier supplier, CancellationToken cancellationToken);
        Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken);
        Task DeleteAsync(long supplierId,CancellationToken cancellationToken);
        IQueryable<Supplier> GetByIdQuery(long supplierId);
        Task<long> CountAsync(SupplierFilter filter, CancellationToken cancellationToken);
        Task<List<Supplier>> GetAllAsync(SupplierFilter filter, int page, int count, CancellationToken cancellationToken);
    }
    public class SupplierService : ISupplierService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationClient _authenticationClient;
       

        public SupplierService(DocumentManagementDbContext dbContext, IIdentityService identityService, IServiceProvider serviceProvider, IAuthenticationClient authenticationClient)
        {
            _dbContext = dbContext;
            _authenticationClient =authenticationClient;
            _identityService=identityService;
            _serviceProvider=serviceProvider;

        }
        public async Task<Supplier> AddAsync(Supplier supplier, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                await _dbContext.AddAsync(supplier,cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
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

            return supplier;
        }

        public async Task<long> CountAsync(SupplierFilter filter, CancellationToken cancellationToken)
        {
            var x = (await GetSupplierExpressions(filter, cancellationToken)).ToPredicates();

            return await _dbContext.Suppliers
                  .WhereAll((await GetSupplierExpressions(filter, cancellationToken)).ToPredicates())
                  .AsNoTracking()
                  .CountAsync(cancellationToken);
        }

        public async Task DeleteAsync(long supplierId, CancellationToken cancellationToken)
        {
            var supplier = await _dbContext.Suppliers.Where(p => p.Id == supplierId).FirstOrDefaultAsync();
            if (supplier != null)
            {
            
            await _dbContext.SingleDeleteAsync(supplier, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
             }

        }

        public async Task<List<Supplier>> GetAllAsync(SupplierFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            return await _dbContext.Suppliers
                .WhereAll((await GetSupplierExpressions(filter, cancellationToken)).ToPredicates())
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * count)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public IQueryable<Supplier> GetByIdQuery(long supplierId)
        {
            return  _dbContext.Suppliers.Where(p => p.Id == supplierId);

        }

        public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
                await _dbContext.SingleUpdateAsync(supplier, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
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

         
        }
        private async Task<DataExpressions<Supplier>> GetSupplierExpressions(SupplierFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Supplier>();

            dataExpressions.AddRange(await GetSupplierExpressionsAsync(filter, token));
          
            return dataExpressions;
        }
   
        private Task<DataExpressions<Supplier>> GetSupplierExpressionsAsync(SupplierFilter filter, CancellationToken token)
        {
            var filterComponent = new SupplierFilterComponent(_serviceProvider);
            var filterComponentContext = new SupplierFilterComponentContext
            {
                SupplierFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }
    }
}
