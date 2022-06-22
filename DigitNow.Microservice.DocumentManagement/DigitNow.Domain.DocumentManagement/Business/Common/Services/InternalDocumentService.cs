using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
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
    public interface IInternalDocumentService
    {
        Task<InternalDocument> CreateAsync(InternalDocument internalDocument, CancellationToken cancellationToken);
        Task<List<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class InternalDocumentService : IInternalDocumentService
    {        
        private readonly IInternalDocumentRepository _internalDocumentRepository;
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;
        private readonly IDocumentResolutionService _documentResolutionService;

        public InternalDocumentService(            
            IInternalDocumentRepository internalDocumentRepository,
            IDocumentService documentService, 
            IIdentityService identityService,
            IDocumentResolutionService documentResolutionService)
        {
            _internalDocumentRepository = internalDocumentRepository;
            _documentService = documentService;
            _identityService = identityService;
            _documentResolutionService = documentResolutionService;
        }

        public async Task<InternalDocument> CreateAsync(InternalDocument internalDocument, CancellationToken cancellationToken)
        {
            internalDocument.Document = new Document
            {
                CreatedAt = DateTime.Now,
                DocumentType = DocumentType.Incoming
            };

            if (_identityService.TryGetCurrentUserId(out int userId))
            {
                internalDocument.CreatedBy = userId;
            }

            await _internalDocumentRepository.InsertAsync(internalDocument, cancellationToken);
            return internalDocument;
        }

        public Task<List<InternalDocument>> FindAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken) =>
            _internalDocumentRepository.FindByAsync(predicate, cancellationToken);

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbInternalDocuments = await _internalDocumentRepository.FindByAsync(x => documentIds.Contains(x.Id), cancellationToken, x => x.Document);

            foreach (var dbInternalDocument in dbInternalDocuments)
            {
                await _documentResolutionService.AddAsync(DocumentResolutionFactory.Create(dbInternalDocument, resolutionType, remarks), cancellationToken);
            }

            await _internalDocumentRepository.CommitAsync(cancellationToken);
        }
    }
}
