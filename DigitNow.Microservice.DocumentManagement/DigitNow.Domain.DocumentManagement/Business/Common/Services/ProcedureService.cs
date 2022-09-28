using DigitNow.Domain.DocumentManagement.Contracts.Objectives;
using DigitNow.Domain.DocumentManagement.Contracts.Scim;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IProcedureService : IScimStateService
    {
        Task<Procedure> AddAsync(Procedure procedure, CancellationToken cancellationToken);
        Task UpdateAsync(Procedure procedure, CancellationToken cancellationToken);
        IQueryable<Procedure> GetByIdQuery(long procedureId);
    }

    public class ProcedureService : ScimStateService, IProcedureService
    {
        public ProcedureService(DocumentManagementDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Procedure> AddAsync(Procedure procedure, CancellationToken cancellationToken)
        {
            procedure.State = ScimState.Active;
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
            return DbContext.Procedures.Where(p => p.Id == procedureId);
        }

        public async Task UpdateAsync(Procedure procedure, CancellationToken cancellationToken)
        {
            await ChangeStateAsync(new List<long> { procedure.Id }, ScimEntity.ScimProcedure, procedure.State, cancellationToken);
            await DbContext.SingleUpdateAsync(procedure, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task SetProcedureCodeAsync(Procedure procedure, CancellationToken token)
        {
            //TODO: implement code
            procedure.Code = $"ProcedureCode";
        }
    }
}
