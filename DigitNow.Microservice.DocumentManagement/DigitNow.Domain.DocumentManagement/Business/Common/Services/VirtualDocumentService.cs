using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services;

public interface IVirtualDocumentService
{
    Task<List<VirtualDocument>> FindAllAsync<T>(
        Expression<Func<VirtualDocument, bool>> predicate,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes) where T : VirtualDocument;
}

public class VirtualDocumentService : IVirtualDocumentService
{
    private readonly DocumentManagementDbContext _dbContext;

    public VirtualDocumentService(DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<VirtualDocument>> FindAllAsync<T>(
        Expression<Func<VirtualDocument, bool>> predicate,
        CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includes) where T : VirtualDocument
    {
        return _dbContext.Set<T>()
             .Includes(includes)
             .Where(predicate)
             .ToListAsync(cancellationToken);
    }
}