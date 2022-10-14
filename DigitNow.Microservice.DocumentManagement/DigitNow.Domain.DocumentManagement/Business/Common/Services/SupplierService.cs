using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.Suppliers;
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
        IQueryable<Supplier> GetByIdQuery(long supplierId);
      //  Task<long> CountAsync(Supplier filter, CancellationToken cancellationToken);
      //  Task<List<Supplier>> GetAllAsync(Supplier filter, int page, int count, CancellationToken cancellationToken);
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


        public IQueryable<Supplier> GetByIdQuery(long supplierId)
        {
            return _dbContext.Suppliers.Where(p => p.Id == supplierId);
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
    }
}
