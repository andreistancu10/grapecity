using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities.SpecialRegisters;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public interface ISpecialRegisterService
{
    Task<SpecialRegister> CreateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
    Task<SpecialRegister> UpdateAsync(SpecialRegister specialRegister, CancellationToken cancellationToken);
    Task<List<SpecialRegister>> FindAllAsync(CancellationToken cancellationToken);
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
}