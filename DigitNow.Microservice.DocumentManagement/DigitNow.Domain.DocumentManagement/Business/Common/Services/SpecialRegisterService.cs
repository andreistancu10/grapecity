using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecialRegisterService
    {
        Task<SpecialRegister> CreateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
        Task<SpecialRegister> UpdateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
        Task<List<SpecialRegister>> FindAllAsync(CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<SpecialRegister, bool>> predicate, CancellationToken cancellationToken);
        Task<SpecialRegister> FindAsync(Expression<Func<SpecialRegister, bool>> predicate, CancellationToken cancellationToken);
    }

    public class SpecialRegisterService : ISpecialRegisterService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public SpecialRegisterService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SpecialRegister> CreateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken)
        {
            await _dbContext.SpecialRegisters.AddAsync(specialRegister, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return specialRegister;
        }

        public async Task<SpecialRegister> UpdateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken)
        {
            _dbContext.SpecialRegisters.Update(specialRegister);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return specialRegister;
        }

        public Task<List<SpecialRegister>> FindAllAsync(CancellationToken cancellationToken)
        {
            return _dbContext.SpecialRegisters.ToListAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<SpecialRegister, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.SpecialRegisters.AnyAsync(predicate, cancellationToken);
        }

        public Task<SpecialRegister> FindAsync(Expression<Func<SpecialRegister, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.SpecialRegisters.FirstAsync(predicate, cancellationToken);
        }
    }
}