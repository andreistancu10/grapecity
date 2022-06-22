using DigitNow.Domain.DocumentManagement.Data.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IDocumentRepository : IEntityRepository<Document>
    {
    }

    public class DocumentRepository : EntityRepository<Document>, IDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public DocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
