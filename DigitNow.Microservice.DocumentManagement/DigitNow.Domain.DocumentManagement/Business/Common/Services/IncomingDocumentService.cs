using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Documents;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIncomingDocumentService
    {
        Task<IncomingDocument> AddAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken);
        Task<IList<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> query, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;

        public IncomingDocumentService(DocumentManagementDbContext dbContext, IDocumentService documentService, IIdentityService identityService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _identityService = identityService;
        }

        public async Task<IncomingDocument> AddAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
        {
            incomingDocument.Document = new Document
            {
                CreatedAt = DateTime.Now,
                CreatedBy = _identityService.GetCurrentUserId(),
                DocumentType = DocumentType.Incoming
            };

            await _dbContext.IncomingDocuments.AddAsync(incomingDocument, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return incomingDocument;
        }

        public async Task<IList<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> query, CancellationToken cancellationToken)
        {
            return await _dbContext.IncomingDocuments
                .Where(query)
                .ToListAsync(cancellationToken);
        }

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbIncomingDocuments = await _dbContext.IncomingDocuments
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var dbIncomingDocument in dbIncomingDocuments)
            {
                await _dbContext.DocumentResolutions
                    .AddAsync(DocumentResolutionFactory.Create(dbIncomingDocument, resolutionType, remarks));
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
