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
        Task<OutgoingDocument> CreateAsync(OutgoingDocument outgoingDocument, CancellationToken cancellationToken);
        Task<List<OutgoingDocument>> FindAllAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken);        
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

        public async Task<OutgoingDocument> CreateAsync(OutgoingDocument outgoingDocument, CancellationToken cancellationToken)
        {
            await _documentService.AddDocument(new Document
            {
                DocumentType = DocumentType.Outgoing,
                OutgoingDocument = outgoingDocument
            }, cancellationToken);

            return outgoingDocument;
        }

        public Task<List<OutgoingDocument>> FindAllAsync(Expression<Func<OutgoingDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.OutgoingDocuments
                .Include(x => x.Document)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }
    }
}
