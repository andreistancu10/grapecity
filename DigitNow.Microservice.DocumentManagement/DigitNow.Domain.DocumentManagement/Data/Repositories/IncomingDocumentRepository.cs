using DigitNow.Domain.DocumentManagement.Data;
using DigitNow.Domain.DocumentManagement.Data.Abstractions;
using DigitNow.Domain.DocumentManagement.Data.Entities;

namespace DigitNow.Domain.DocumentManagement.Data.Repositories
{
    public interface IIncomingDocumentRepository : IEntityRepository<IncomingDocument>
    {
    }

    public class IncomingDocumentRepository : EntityRepository<IncomingDocument>, IIncomingDocumentRepository
    {
        private readonly DocumentManagementDbContext _dbContext;

        public IncomingDocumentRepository(DocumentManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
