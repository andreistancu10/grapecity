using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services;

public interface IDocumentService
{
    Task<Document> AddAsync(Document newDocument, CancellationToken cancellationToken);
    Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);
    Task<List<Document>> FindAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);

    IQueryable<Document> FindAllQueryable(Expression<Func<Document, bool>> predicate);
    Task<int> CountAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);

    [Obsolete("This will be refactored in future sprints", error: false)]
    Task AddDocument(Document document, CancellationToken cancellationToken);
}

public class DocumentService : IDocumentService
{
    protected readonly DocumentManagementDbContext _dbContext;
    private readonly IIdentityService _identityService;

    public DocumentService(DocumentManagementDbContext dbContext,
        IIdentityService identityService)
    {
        _dbContext = dbContext;
        _identityService = identityService;
    }

    public async Task<Document> AddAsync(Document newDocument, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(newDocument, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newDocument;
    }

    public Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbContext.Documents
            .FirstOrDefaultAsync(predicate);
    }

    public Task<List<Document>> FindAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbContext.Documents
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public IQueryable<Document> FindAllQueryable(Expression<Func<Document, bool>> predicate)
    {
        return _dbContext.Documents
            .Where(predicate);
    }

    public Task<int> CountAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbContext.Documents
            .Where(predicate)
            .CountAsync(cancellationToken);
    }

    // TODO: Use RegistrationNumberCounterService
    public async Task AddDocument(Document document, CancellationToken cancellationToken)
    {
        var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var maxRegNumber = await _dbContext.RegistrationNumberCounters
                .Where(reg => reg.RegistrationDate.Year == DateTime.Now.Year)
                .Select(reg => reg.RegistrationNumber)
                .DefaultIfEmpty()
                .MaxAsync();

            var newRegNumber = ++maxRegNumber;

            document.RegistrationNumber = newRegNumber;
            document.RegistrationDate = DateTime.Now;

            if (document.InternalDocument == null && document.IncomingDocument == null && document.OutgoingDocument == null)
                throw new InvalidOperationException(); //TODO: Add descriptive error

            await _dbContext.AddAsync(document, cancellationToken);
            await _dbContext.RegistrationNumberCounters.AddAsync(new RegistrationNumberCounter
            {
                RegistrationNumber = newRegNumber,
                RegistrationDate = DateTime.Now
            });

            await _dbContext.SaveChangesAsync(cancellationToken);
            await dbContextTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            //TODO: Log error
            await dbContextTransaction.RollbackAsync();
            throw;
        }
        finally
        {
            dbContextTransaction?.Dispose();
        }
    }
}