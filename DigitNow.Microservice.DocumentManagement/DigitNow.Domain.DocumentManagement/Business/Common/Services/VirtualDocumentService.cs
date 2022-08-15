using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Services
{
    public interface IVirtualDocumentService
    {
        Task<List<VirtualDocument>> FindAllAsync<T>(
            Expression<Func<VirtualDocument, bool>> predicate,
            CancellationToken token,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument;

        IList<VirtualDocument> ConvertDocumentsToVirtualDocuments(IList<Document> documents);
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
            CancellationToken token,
            params Expression<Func<T, object>>[] includes) where T : VirtualDocument
        {
            return _dbContext.Set<T>()
                .Includes(includes)
                .Where(predicate)
                .ToListAsync(token);
        }

        public IList<VirtualDocument> ConvertDocumentsToVirtualDocuments(IList<Document> documents)
        {
            var result = new List<VirtualDocument>();

            result.AddRange(documents.Select(x => x.IncomingDocument).Where(x => x != null));
            result.AddRange(documents.Select(x => x.InternalDocument).Where(x => x != null));
            result.AddRange(documents.Select(x => x.OutgoingDocument).Where(x => x != null));

            return result;
        }
    }
}