using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Data.RegistrationNumberCounters;
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
    Task<IList<Document>> FindAsync(Expression<Func<Document, bool>> query, CancellationToken cancellationToken);
    Task AssignRegistrationNumberAsync(Document document);
}

public class DocumentService : IDocumentService
{
    protected readonly DocumentManagementDbContext _dbContext;
    private readonly IIdentityService _identityService;

    public DocumentService(DocumentManagementDbContext dbContext, IIdentityService identityService)
    {
        _dbContext = dbContext;
        _identityService = identityService;
    }

    public async Task<IList<Document>> FindAsync(Expression<Func<Document, bool>> query, CancellationToken cancellationToken)
    {
        return await _dbContext.Documents
            .Where(query)
            .ToListAsync(cancellationToken);
    }

    public async Task AssignRegistrationNumberAsync(Document document)
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

            document.RegistrationNumber = ++maxRegNumber;
            document.CreatedAt = DateTime.Now;
            document.CreatedBy = _identityService.GetCurrentUserId();

            await _dbContext.AddAsync(document);
            await _dbContext.RegistrationNumberCounters.AddAsync(new RegistrationNumberCounter 
            { 
                RegistrationNumber = newRegNumber, 
                RegistrationDate = DateTime.Now                
            });

            await _dbContext.SaveChangesAsync();
            await dbContextTransaction.CommitAsync();
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