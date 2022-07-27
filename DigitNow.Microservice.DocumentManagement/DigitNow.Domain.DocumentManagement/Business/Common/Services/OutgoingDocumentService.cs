using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IOutgoingDocumentService
    {
        Task<OutgoingDocument> AddAsync(OutgoingDocument outgoingDocument, CancellationToken cancellationToken);
        Task<List<OutgoingDocument>> FindAllAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken);
        Task<OutgoingDocument> FindFirstAsync(long id, CancellationToken cancellationToken);
    }

    public class OutgoingDocumentService : IOutgoingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public OutgoingDocumentService(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }

        public async Task<OutgoingDocument> AddAsync(OutgoingDocument outgoingDocument, CancellationToken cancellationToken)
        {
            if (outgoingDocument.Document == null)
            {
                outgoingDocument.Document = new Document();
            }

            outgoingDocument.Document.DocumentType = DocumentType.Outgoing;
            outgoingDocument.Document.RegistrationDate = DateTime.Now;
            outgoingDocument.Document.Status = DocumentStatus.New;
            outgoingDocument.Document.RecipientId = null;

            await _documentService.AddAsync(outgoingDocument.Document, cancellationToken);            
            await _dbContext.AddAsync(outgoingDocument, cancellationToken);            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return outgoingDocument;
        }

        public Task<List<OutgoingDocument>> FindAllAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.OutgoingDocuments
                .Include(x => x.Document)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public Task<OutgoingDocument> FindFirstAsync(long id, CancellationToken cancellationToken)
        {
            return _dbContext.OutgoingDocuments.Where(x => x.DocumentId == id).Include(x => x.Document).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
