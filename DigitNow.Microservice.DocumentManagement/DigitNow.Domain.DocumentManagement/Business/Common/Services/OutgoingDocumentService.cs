using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        private readonly IIdentityService _identityService;

        public OutgoingDocumentService(
            DocumentManagementDbContext dbContext,
            IDocumentService documentService,
            IIdentityService identityService)
        {
            _dbContext = dbContext;
            _documentService = documentService;
            _identityService = identityService;
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

            if (!outgoingDocument.Document.RecipientId.HasValue)
            {
                outgoingDocument.Document.RecipientId = await _identityService.GetHeadOfDepartmentUserIdAsync(outgoingDocument.Document.DestinationDepartmentId, cancellationToken);
            }

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
