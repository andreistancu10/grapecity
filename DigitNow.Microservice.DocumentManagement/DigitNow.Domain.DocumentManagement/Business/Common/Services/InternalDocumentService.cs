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
    public interface IInternalDocumentService
    {
        Task<InternalDocument> AddAsync(InternalDocument internalDocument, CancellationToken cancellationToken);
        Task<List<InternalDocument>> FindAllAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
    }

    public class InternalDocumentService : IInternalDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;

        public InternalDocumentService(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _identityService = identityService;
        }

        public async Task<InternalDocument> AddAsync(InternalDocument internalDocument, CancellationToken cancellationToken)
        {
            if (internalDocument.Document == null)
            {
                internalDocument.Document = new Document();
            }

            internalDocument.Document.DocumentType = DocumentType.Internal;
            internalDocument.Document.RegistrationDate = DateTime.Now;
            internalDocument.Document.Status = DocumentStatus.New;

            if (!internalDocument.Document.RecipientId.HasValue)
            {
                internalDocument.Document.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(internalDocument.Document.DestinationDepartmentId, cancellationToken);
            }

            await _documentService.AddAsync(internalDocument.Document, cancellationToken);
            await _dbContext.AddAsync(internalDocument, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return internalDocument;
        }

        public Task<List<InternalDocument>> FindAllAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbContext.InternalDocuments
                .Include(x => x.Document)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task SetResolutionAsync(IList<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbInternalDocuments = await _dbContext.InternalDocuments
                .Include(x => x.Document)
                .Where(x => documentIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (!dbInternalDocuments.Any()) return;

            foreach (var dbInternalDocument in dbInternalDocuments)
            {
                var foundResolution = await _dbContext.DocumentResolutions
                    .FirstOrDefaultAsync(x => x.DocumentId == dbInternalDocument.DocumentId, cancellationToken);

                if (foundResolution == null)
                {
                    await _dbContext.AddAsync(DocumentResolutionFactory.Create(dbInternalDocument, resolutionType, remarks), cancellationToken);
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
