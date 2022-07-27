﻿using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
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
        Task<List<IncomingDocument>> FindAllAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
        Task<IncomingDocument> FindFirstAsync(long id, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public IncomingDocumentService(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
        }

        public async Task<IncomingDocument> AddAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken)
        {
            if (incomingDocument.Document == null)
            {
                incomingDocument.Document = new Document();
            }

            incomingDocument.Document.DocumentType = DocumentType.Incoming;
            incomingDocument.Document.RegistrationDate = DateTime.Now;
            incomingDocument.Document.Status = DocumentStatus.InWorkUnallocated;
            incomingDocument.Document.RecipientId = null;

            await _documentService.AddAsync(incomingDocument.Document, cancellationToken);
            await _dbContext.AddAsync(incomingDocument, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return incomingDocument;
        }

        public Task<List<IncomingDocument>> FindAllAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.IncomingDocuments
                .Include(x => x.Document)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public Task<IncomingDocument> FindFirstAsync(long id, CancellationToken cancellationToken)
        {
            return _dbContext.IncomingDocuments.Where(x => x.DocumentId == id).Include(x => x.Document).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbIncomingDocuments = await _dbContext.IncomingDocuments
                .Include(x => x.Document)
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!dbIncomingDocuments.Any()) return;

            foreach (var dbIncomingDocument in dbIncomingDocuments)
            {
                var foundResolution = await _dbContext.DocumentResolutions
                    .FirstOrDefaultAsync(x => x.DocumentId == dbIncomingDocument.DocumentId, cancellationToken);                    

                if (foundResolution == null)
                {
                    await _dbContext.AddAsync(DocumentResolutionFactory.Create(dbIncomingDocument, resolutionType, remarks), cancellationToken);
                }
                else
                {
                    foundResolution.ResolutionType = resolutionType;
                    foundResolution.Remarks = remarks;

                    await _dbContext.SingleUpdateAsync(foundResolution, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
