using DigitNow.Domain.DocumentManagement.Business.Common.Factories;
using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IInternalDocumentService
    {
        Task<InternalDocument> AddAsync(InternalDocument internalDocument, CancellationToken cancellationToken);
        Task<List<InternalDocument>> FindAllAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
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

        public async Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbInternalDocuments = await _dbContext.InternalDocuments
                .Include(x => x.Document)
                .Where(x => documentIds.Contains(x.DocumentId))
                .ToListAsync(cancellationToken);
            if (!dbInternalDocuments.Any()) return;

            var foundResolutions = await _dbContext.DocumentResolutions
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync(cancellationToken);

            foreach (var dbInternalDocument in dbInternalDocuments)
            {
                var foundResolution = foundResolutions.FirstOrDefault(x => x.DocumentId == dbInternalDocument.DocumentId);
                if (foundResolution == null)
                {
                    await _dbContext.DocumentResolutions
                        .AddAsync(DocumentResolutionFactory.Create(dbInternalDocument, resolutionType, remarks), cancellationToken);
                }
                else
                {
                    foundResolution.ResolutionType = resolutionType;
                    foundResolution.Remarks = remarks;

                    await _dbContext.DocumentResolutions
                        .SingleUpdateAsync(foundResolution, cancellationToken);
                }

                dbInternalDocument.Document.Status = DocumentStatus.Finalized;
                await _dbContext.Documents.SingleUpdateAsync(dbInternalDocument.Document, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
