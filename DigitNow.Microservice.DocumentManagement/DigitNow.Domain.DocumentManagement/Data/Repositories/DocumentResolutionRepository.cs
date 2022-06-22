using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IDocumentResolutionRepository : IEntityRepository<DocumentResolution>
    {
    }

    internal class DocumentResolutionRepository : EntityRepository<DocumentResolution>, IDocumentResolutionRepository
    {
        private DocumentManagementDbContext _dbContext;

        public DocumentResolutionRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
