using DigitNow.Domain.DocumentManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IDocumentResolutionService
    {
        Task<DocumentResolution> AddAsync(DocumentResolution newDocumentResolution, CancellationToken cancellationToken);
        Task<DocumentResolution> FindByDocumentIdAsync(long documentId, CancellationToken cancellationToken);
    }

    public class DocumentResolutionService : IDocumentResolutionService
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentResolutionService(
            DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DocumentResolution> AddAsync(DocumentResolution newDocumentResolution, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(newDocumentResolution, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newDocumentResolution;
        }

        public Task<DocumentResolution> FindByDocumentIdAsync(long documentId, CancellationToken cancellationToken)
        {
            return _dbContext.DocumentResolutions.FirstOrDefaultAsync(x => x.DocumentId == documentId, cancellationToken);                
        }
    }
}
