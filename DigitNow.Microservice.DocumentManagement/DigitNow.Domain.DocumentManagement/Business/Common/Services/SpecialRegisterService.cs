

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Business.SpecialRegisters.Queries.GetSpecialRegisters;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using HTSS.Platform.Infrastructure.Data.Abstractions;
using HTSS.Platform.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface ISpecialRegisterService
    {
        Task<SpecialRegister> CreateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
        Task<SpecialRegister> UpdateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
        Task<PagedList<SpecialRegister>> FindAllAsync(GetSpecialRegistersQuery request, CancellationToken cancellationToken);
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

        public async Task<PagedList<SpecialRegister>> FindAllAsync(GetSpecialRegistersQuery request, CancellationToken cancellationToken)
        {
            var descriptor = new FilterDescriptor<SpecialRegister>(_dbContext.SpecialRegisters.AsNoTracking(), request.SortField, request.SortOrder);

            descriptor.Query(p => p.Id, request.Id, () => request.GetFilterMode(p => p.Id));
            descriptor.Query(p => p.Name, request.Name, () => request.GetFilterMode(p => p.Name));

            var pagedResult = await descriptor.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);

            return pagedResult;
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