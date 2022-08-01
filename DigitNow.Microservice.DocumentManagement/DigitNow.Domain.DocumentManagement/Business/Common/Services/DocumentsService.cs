using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IDocumentService
    {
        Task<Document> AddAsync(Document newDocument, CancellationToken cancellationToken);
        Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Document, object>>[] includes);
        Task<List<Document>> FindAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);

        IQueryable<Document> FindAllQueryable(Expression<Func<Document, bool>> predicate);
        Task<int> CountAllAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken);
    }

    public class DocumentService : IDocumentService
    {
        protected readonly DocumentManagementDbContext _dbContext;

        public DocumentService(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Document> AddAsync(Document newDocument, CancellationToken cancellationToken)
        {
            var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                // Insert the entity without relationships
                _dbContext.Entry(newDocument).State = EntityState.Added;
                await SetRegistrationNumberAsync(newDocument, cancellationToken);
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

            return newDocument;
        }

        public Task<Document> FindAsync(Expression<Func<Document, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<Document, object>>[] includes)
        {
            return _dbContext.Documents
                .Includes(includes)
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

        private async Task SetRegistrationNumberAsync(Document document, CancellationToken cancellationToken)
        {
            var maxRegNumber = await _dbContext.Documents
                .Where(reg => reg.RegistrationDate.Year == DateTime.Now.Year)
                .Select(reg => reg.RegistrationNumber)
                .DefaultIfEmpty()
                .MaxAsync();

            document.RegistrationNumber = ++maxRegNumber;
            document.RegistrationDate = DateTime.Now;
        }
    }
}