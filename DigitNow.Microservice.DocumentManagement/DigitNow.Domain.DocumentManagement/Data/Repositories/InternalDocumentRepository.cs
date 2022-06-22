using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IInternalDocumentRepository : IEntityRepository<InternalDocument>
    {
    }

    public class InternalDocumentRepository : EntityRepository<InternalDocument>, IInternalDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public InternalDocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
