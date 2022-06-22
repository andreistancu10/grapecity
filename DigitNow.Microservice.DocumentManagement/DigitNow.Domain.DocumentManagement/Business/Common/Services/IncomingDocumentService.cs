using DigitNow.Domain.DocumentManagement.Business.Common.Repositories;
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
        Task<IncomingDocument> CreateAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken);
        Task<List<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IIncomingDocumentRepository _incomingDocumentRepository;
        private readonly IIdentityService _identityService;

        public IncomingDocumentService(DocumentManagementDbContext dbContext, 
            IDocumentService documentService, 
            IIncomingDocumentRepository incomingDocumentRepository,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _incomingDocumentRepository = incomingDocumentRepository;
            _identityService = identityService;
        }

        public async Task<IncomingDocument> CreateAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
        {
            incomingDocument.Document = new Document
            {
                CreatedAt = DateTime.Now,
                CreatedBy = _identityService.GetCurrentUserId(),
                DocumentType = DocumentType.Incoming
            };

            if (_identityService.TryGetCurrentUserId(out int userId))
            {
                incomingDocument.CreatedBy = userId;
            }

            await _incomingDocumentRepository.InsertAsync(incomingDocument, cancellationToken);
            return incomingDocument;
        }

        public Task<List<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken) =>
            _incomingDocumentRepository.FindByAsync(predicate, cancellationToken);

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbIncomingDocuments = await _dbContext.IncomingDocuments
                .Include(x => x.Document)
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
