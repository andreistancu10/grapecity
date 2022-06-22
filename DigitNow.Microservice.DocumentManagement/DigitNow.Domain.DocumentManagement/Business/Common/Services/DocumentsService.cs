using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Repositories;
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
    Task<List<Document>> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);
    Task AssignRegistrationNumberAsync(Document document);
}

public class DocumentService : IDocumentService
{
    protected readonly DocumentManagementDbContext _dbContext;
    private readonly IDocumentRepository _documentRepository;
    private readonly IIdentityService _identityService;

    public DocumentService(DocumentManagementDbContext dbContext, 
        IDocumentRepository documentRepository,
        IIdentityService identityService)
    {
        _dbContext = dbContext;
        _documentRepository = documentRepository;
        _identityService = identityService;
    }

    public async Task<Document> AddAsync(Document newDocument, CancellationToken cancellationToken)
    {
        await _documentRepository.InsertAsync(newDocument, cancellationToken);
        await _documentRepository.CommitAsync(cancellationToken);
        return newDocument;
    }

    public Task<List<Document>> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken) => 
        _documentRepository.FindByAsync(predicate, cancellationToken);
    
    // TODO: Use RegistrationNumberCounterService
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