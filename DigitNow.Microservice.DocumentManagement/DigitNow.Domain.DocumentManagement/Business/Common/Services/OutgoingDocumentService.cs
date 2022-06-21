using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IOutgoingDocumentService
    {
        Task<IList<OutgoingDocument>> FindAsync(Expression<Func<OutgoingDocument, bool>> query, CancellationToken cancellationToken);
    }

    public class OutgoingDocumentService : IOutgoingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public OutgoingDocumentService(DocumentManagementDbContext dbContext, IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }

        public async Task<IList<OutgoingDocument>> FindAsync(Expression<Func<OutgoingDocument, bool>> query, CancellationToken cancellationToken)
        {
            return await _dbContext.OutgoingDocuments
                .Where(query)
                .ToListAsync(cancellationToken);
        }
    }
}
