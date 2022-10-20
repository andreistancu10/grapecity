using System.Data;
using DigitNow.Adapters.MS.Catalog;
using DigitNow.Domain.Authentication.Client;
using DigitNow.Domain.Authentication.Contracts.LegalEntities.GetLegalEntity;
using DigitNow.Domain.Catalog.Client;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures;
using DigitNow.Domain.DocumentManagement.Contracts.Procedures;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using Microsoft.EntityFrameworkCore;
using System.Text;
using AutoMapper;
using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureHistoryService
    {
        Task<ProcedureHistory> AddAsync(ProcedureHistory procedureHistory, CancellationToken cancellationToken);
        Task<ProcedureHistory> AddAsync(Procedure procedure, CancellationToken cancellationToken);
        Task UpdateAsync(ProcedureHistory procedureHistory, CancellationToken cancellationToken);
        IQueryable<ProcedureHistory> GetByIdQuery(long procedureId);
        Task<long> CountAsync(ProcedureHistoryFilter filter, CancellationToken cancellationToken);
        Task<List<ProcedureHistory>> GetAllAsync(ProcedureHistoryFilter filter, int page, int count, CancellationToken cancellationToken);
    }

    public class ProcedureHistoryService : IProcedureHistoryService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public ProcedureHistoryService(
            DocumentManagementDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _serviceProvider = serviceProvider;
        }

        public async Task<ProcedureHistory> AddAsync(ProcedureHistory procedureHistory, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            try
            {
                _dbContext.Entry(procedureHistory).State = EntityState.Added;

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
                dbContextTransaction.Dispose();
            }

            return procedureHistory;
        }

        public async Task<ProcedureHistory> AddAsync(Procedure procedure, CancellationToken cancellationToken)
        {
            var procedureHistory = new ProcedureHistory
            {
                ProcedureId = procedure.Id,
                Revision = procedure.Revision,
                Edition = procedure.Edition
            };

            return await AddAsync(procedureHistory, cancellationToken);
        }

        public IQueryable<ProcedureHistory> GetByIdQuery(long procedureId)
        {
            return _dbContext.ProcedureHistories.Where(p => p.Id == procedureId);
        }

        public async Task UpdateAsync(ProcedureHistory procedureHistory, CancellationToken cancellationToken)
        {
            await _dbContext.SingleUpdateAsync(procedureHistory, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<long> CountAsync(ProcedureHistoryFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.ProcedureHistories
                .WhereAll((await GetProceduresExpressions(filter, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        public async Task<List<ProcedureHistory>> GetAllAsync(ProcedureHistoryFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            return await _dbContext.ProcedureHistories
                 .WhereAll((await GetProceduresExpressions(filter, cancellationToken)).ToPredicates())
                 .Include(c => c.Procedure)
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);
        }

        private async Task<DataExpressions<ProcedureHistory>> GetProceduresExpressions(ProcedureHistoryFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<ProcedureHistory>();

            dataExpressions.AddRange(await GetProceduresExpressionsAsync(filter, token));

            return dataExpressions;
        }

        private Task<DataExpressions<ProcedureHistory>> GetProceduresExpressionsAsync(ProcedureHistoryFilter filter, CancellationToken token)
        {
            var filterComponent = new ProcedureHistoriesFilterComponent(_serviceProvider);
            var filterComponentContext = new ProcedureHistoriesFilterComponentContext
            {
                ProcedureHistoryFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }
    }
}