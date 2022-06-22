using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Data.Repositories;
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
        Task<List<IncomingDocument>> FindAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IIncomingDocumentRepository _incomingDocumentRepository;
        private readonly IIdentityService _identityService;
        private readonly IDocumentResolutionRepository _documentResolutionRepository;

        public IncomingDocumentService(DocumentManagementDbContext dbContext, 
            IDocumentService documentService, 
            IIncomingDocumentRepository incomingDocumentRepository,
            IIdentityService identityService,
            IDocumentResolutionRepository documentResolutionRepository)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _incomingDocumentRepository = incomingDocumentRepository;
            _identityService = identityService;
            _documentResolutionRepository = documentResolutionRepository;
        }

        public async Task<IncomingDocument> AddAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
        {
            incomingDocument.Document = new Document
            {
                CreatedAt = DateTime.Now,
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
            var dbIncomingDocuments = await _incomingDocumentRepository.FindByAsync(x => documentIds.Contains(x.Id), cancellationToken, x => x.Document);

            foreach (var dbIncomingDocument in dbIncomingDocuments)
            {
                await _documentResolutionRepository.InsertAsync(DocumentResolutionFactory.Create(dbIncomingDocument, resolutionType, remarks), cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
