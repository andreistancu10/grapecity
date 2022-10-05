using DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;
using DigitNow.Domain.DocumentManagement.Business.Common.Filters.Components.Procedures;
using DigitNow.Domain.DocumentManagement.Business.Common.Models;
using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using DigitNow.Domain.DocumentManagement.Data.Filters;
using DigitNow.Domain.DocumentManagement.Data.Filters.Procedures;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DigitNow.Domain.Catalog.Client;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureService : IScimStateService
    {
        Task<Procedure> AddAsync(Procedure procedure, CancellationToken cancellationToken);
        Task UpdateAsync(Procedure procedure, CancellationToken cancellationToken);
        IQueryable<Procedure> GetByIdQuery(long procedureId);
        Task<long> CountAsync(ProcedureFilter filter, CancellationToken cancellationToken);
        Task<List<Procedure>> GetAllAsync(ProcedureFilter filter, int page, int count, CancellationToken cancellationToken);
    }

    public class ProcedureService : ScimStateService, IProcedureService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICatalogClient _catalogClient;

        public ProcedureService(
            DocumentManagementDbContext dbContext,
            IIdentityService identityService,
            IServiceProvider serviceProvider, ICatalogClient catalogClient) : base(dbContext)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _serviceProvider = serviceProvider;
            _catalogClient = catalogClient;
        }

        public async Task<Procedure> AddAsync(Procedure procedure, CancellationToken cancellationToken)
        {
            var activeScimState = await _catalogClient.ScimStates.GetScimStateByCodeAsync("activ", cancellationToken);
            procedure.StateId = activeScimState.Id;
            var dbContextTransaction = await DbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            try
            {
                DbContext.Entry(procedure).State = EntityState.Added;
                await SetProcedureCodeAsync(procedure, cancellationToken);
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

            return procedure;
        }

        public IQueryable<Procedure> GetByIdQuery(long procedureId)
        {
            return _dbContext.Procedures.Where(p => p.Id == procedureId);
        }

        public async Task UpdateAsync(Procedure procedure, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { procedure.Id }, ScimEntity.ScimProcedure, procedure.StateId, cancellationToken);
            await DbContext.SingleUpdateAsync(procedure, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task SetProcedureCodeAsync(Procedure procedure, CancellationToken token)
        {
            //TODO: implement code
            procedure.Code = $"ProcedureCode";
        }

        public async Task<long> CountAsync(ProcedureFilter filter, CancellationToken cancellationToken)
        {
            return await _dbContext.Procedures
                .WhereAll((await GetProceduresExpressions(filter, cancellationToken)).ToPredicates())
                .AsNoTracking()
                .CountAsync(cancellationToken);
        }

        private async Task<DataExpressions<Procedure>> GetProceduresExpressions(ProcedureFilter filter, CancellationToken token)
        {
            var dataExpressions = new DataExpressions<Procedure>();

            dataExpressions.AddRange(await GetProceduresExpressionsAsync(filter, token));
            dataExpressions.AddRange(await GetProceduresUserRightsExpressionsAsync(token));

            return dataExpressions;
        }
        private async Task<DataExpressions<Procedure>> GetProceduresUserRightsExpressionsAsync(CancellationToken token)
        {
            var currentUser = await _identityService.GetCurrentUserAsync(token);

            var rightsComponent = new ProceduresPermissionsFilterComponent(_serviceProvider);
			var rightsComponentContext = new ProceduresPermissionsFilterComponentContext
			{
				CurrentUser = currentUser
			};

			return await rightsComponent.ExtractDataExpressionsAsync(rightsComponentContext, token);
		}
		private Task<DataExpressions<Procedure>> GetProceduresExpressionsAsync(ProcedureFilter filter, CancellationToken token)
        {
            var filterComponent = new ProceduresFilterComponent(_serviceProvider);
            var filterComponentContext = new ProceduresFilterComponentContext
            {
                ProcedureFilter = filter
            };

            return filterComponent.ExtractDataExpressionsAsync(filterComponentContext, token);
        }

        public async Task<List<Procedure>> GetAllAsync(ProcedureFilter filter, int page, int count, CancellationToken cancellationToken)
        {
            return await _dbContext.Procedures
                 .WhereAll((await GetProceduresExpressions(filter, cancellationToken)).ToPredicates())
                 .OrderByDescending(x => x.CreatedAt)
                 .Skip((page - 1) * count)
                 .Take(count)
                 .AsNoTracking()
                 .ToListAsync(cancellationToken);
        }
    }
}
