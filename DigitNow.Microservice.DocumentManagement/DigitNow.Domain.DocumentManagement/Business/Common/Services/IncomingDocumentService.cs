using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DigitNow.Domain.DocumentManagement.Data.Entities.Documents;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IIncomingDocumentService
    {
        Task<IncomingDocument> AddAsync(IncomingDocument incomingDocument, CancellationToken cancellationToken);
        Task<List<IncomingDocument>> FindAllAsync(Expression<Func<IncomingDocument, bool>> predicate, CancellationToken cancellationToken);
        Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken);
        Task<IncomingDocument> FindFirstAsync(long id, CancellationToken cancellationToken);
    }

    public class IncomingDocumentService : IIncomingDocumentService
    {
        private readonly DocumentManagementDbContext _dbContext;
        private readonly IDocumentService _documentService;
        private readonly IIdentityService _identityService;

        public IncomingDocumentService(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _identityService = identityService;
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

            if (!incomingDocument.Document.RecipientId.HasValue)
            {
                incomingDocument.Document.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(incomingDocument.Document.DestinationDepartmentId, cancellationToken);
            }

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

        public async Task SetResolutionAsync(IEnumerable<long> documentIds, DocumentResolutionType resolutionType, string remarks, CancellationToken cancellationToken)
        {
            var dbIncomingDocuments = await _dbContext.IncomingDocuments
                .Include(x => x.Document)
                .Where(x => documentIds.Contains(x.DocumentId))
                .ToListAsync(cancellationToken);
            if (!dbIncomingDocuments.Any()) return;

            var foundResolutions = await _dbContext.DocumentResolutions
                    .Where(x => documentIds.Contains(x.DocumentId))
                    .ToListAsync(cancellationToken);

            foreach (var dbIncomingDocument in dbIncomingDocuments)
            {
                var foundResolution = foundResolutions.FirstOrDefault(x => x.DocumentId == dbIncomingDocument.DocumentId);
                if (foundResolution == null)
                {
                    await _dbContext.DocumentResolutions
                        .AddAsync(DocumentResolutionFactory.Create(dbIncomingDocument, resolutionType, remarks), cancellationToken);
                }
                else
                {
                    foundResolution.ResolutionType = resolutionType;
                    foundResolution.Remarks = remarks;

                    await _dbContext.DocumentResolutions
                        .SingleUpdateAsync(foundResolution, cancellationToken);
                }

                dbIncomingDocument.Document.Status = DocumentStatus.Finalized;
                await _dbContext.Documents.SingleUpdateAsync(dbIncomingDocument.Document, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
