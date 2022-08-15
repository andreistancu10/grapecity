using DigitNow.Domain.DocumentManagement.Contracts.Documents.Enums;
using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Entities;
using DigitNow.Domain.DocumentManagement.Domain.Business.Common.Factories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Documents.Services
{
    public interface IInternalDocumentService
    {
        Task<InternalDocument> AddAsync(InternalDocument internalDocument, CancellationToken cancellationToken);
        Task<List<InternalDocument>> FindAllAsync(Expression<Func<InternalDocument, bool>> predicate, CancellationToken cancellationToken);
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
    }
}
